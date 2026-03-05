using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace WindowsMcp.Analytics;

public interface IAnalytics : IAsyncDisposable
{
    Task TrackToolAsync(string toolName, Dictionary<string, object> result);
    Task TrackErrorAsync(Exception error, Dictionary<string, object> context);
    Task<bool> IsFeatureEnabledAsync(string feature);
}

public class PostHogAnalytics : IAnalytics
{
    private const string ApiKey = "phc_uxdCItyVTjXNU0sMPr97dq3tcz39scQNt3qjTYw5vLV";
    private const string Host = "https://us.i.posthog.com";

    private static readonly string TempFolder = Path.GetTempPath();

    private readonly HttpClient _httpClient;
    private readonly ILogger<PostHogAnalytics> _logger;
    private readonly string _mcpInteractionId;
    private readonly string _mode;
    private string? _userId;

    public PostHogAnalytics(HttpClient httpClient, ILogger<PostHogAnalytics> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _mcpInteractionId = $"mcp_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{Environment.ProcessId}";
        _mode = Environment.GetEnvironmentVariable("MODE")?.ToLowerInvariant() ?? "local";

        _logger.LogDebug(
            "Initialized with user ID: {UserId} and session ID: {SessionId}",
            UserId, _mcpInteractionId);
    }

    public string UserId
    {
        get
        {
            if (_userId is not null)
                return _userId;

            var userIdFile = Path.Combine(TempFolder, ".windows-mcp-user-id");
            if (File.Exists(userIdFile))
            {
                _userId = File.ReadAllText(userIdFile).Trim();
            }
            else
            {
                _userId = Guid.NewGuid().ToString();
                try
                {
                    File.WriteAllText(userIdFile, _userId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Could not persist user ID: {Error}", ex.Message);
                }
            }

            return _userId;
        }
    }

    public async Task TrackToolAsync(string toolName, Dictionary<string, object> result)
    {
        var properties = new Dictionary<string, object>
        {
            ["tool_name"] = toolName,
            ["session_id"] = _mcpInteractionId,
            ["mode"] = _mode,
            ["process_person_profile"] = true,
        };
        foreach (var kvp in result)
            properties[kvp.Key] = kvp.Value;

        await CaptureAsync("tool_executed", properties);

        result.TryGetValue("duration_ms", out var durationObj);
        var duration = durationObj is long ms ? ms : 0;
        result.TryGetValue("success", out var successObj);
        var successMark = successObj is true ? "SUCCESS" : "FAILED";

        _logger.LogInformation("{ToolName}: {Status} ({Duration}ms)", toolName, successMark, duration);
    }

    public async Task TrackErrorAsync(Exception error, Dictionary<string, object> context)
    {
        var properties = new Dictionary<string, object>
        {
            ["exception"] = error.ToString(),
            ["traceback"] = error.StackTrace ?? error.ToString(),
            ["session_id"] = _mcpInteractionId,
            ["mode"] = _mode,
            ["process_person_profile"] = true,
        };
        foreach (var kvp in context)
            properties[kvp.Key] = kvp.Value;

        await CaptureAsync("exception", properties);

        context.TryGetValue("tool_name", out var toolNameObj);
        _logger.LogError("ERROR in {ToolName}: {Error}", toolNameObj, error.Message);
    }

    public async Task<bool> IsFeatureEnabledAsync(string feature)
    {
        try
        {
            var url = $"{Host}/decide/?v=3";
            var payload = new
            {
                api_key = ApiKey,
                distinct_id = UserId,
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload);
            if (!response.IsSuccessStatusCode)
                return false;

            using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            if (doc.RootElement.TryGetProperty("featureFlags", out var flags)
                && flags.TryGetProperty(feature, out var flagValue))
            {
                return flagValue.ValueKind == JsonValueKind.True
                    || (flagValue.ValueKind == JsonValueKind.String
                        && flagValue.GetString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true);
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to check feature flag {Feature}: {Error}", feature, ex.Message);
            return false;
        }
    }

    public ValueTask DisposeAsync()
    {
        _logger.LogDebug("Closed analytics");
        return ValueTask.CompletedTask;
    }

    private async Task CaptureAsync(string eventName, Dictionary<string, object> properties)
    {
        try
        {
            var payload = new
            {
                api_key = ApiKey,
                @event = eventName,
                distinct_id = UserId,
                properties,
                timestamp = DateTime.UtcNow.ToString("o"),
            };

            var response = await _httpClient.PostAsJsonAsync($"{Host}/capture/", payload);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to send analytics event {Event}: {Error}", eventName, ex.Message);
        }
    }

    /// <summary>
    /// Wraps a tool invocation with analytics tracking (timing, success/failure, error reporting).
    /// Equivalent of the Python <c>with_analytics</c> decorator.
    /// </summary>
    public async Task<T> WithAnalyticsAsync<T>(
        string toolName,
        Func<Task<T>> toolFunc,
        Dictionary<string, object>? extraProperties = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var clientData = extraProperties ?? [];

        try
        {
            var result = await toolFunc();
            stopwatch.Stop();

            var props = new Dictionary<string, object>(clientData)
            {
                ["duration_ms"] = stopwatch.ElapsedMilliseconds,
                ["success"] = true,
            };
            await TrackToolAsync(toolName, props);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            var ctx = new Dictionary<string, object>(clientData)
            {
                ["tool_name"] = toolName,
                ["duration_ms"] = stopwatch.ElapsedMilliseconds,
            };
            await TrackErrorAsync(ex, ctx);

            throw;
        }
    }
}
