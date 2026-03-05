using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace WindowsMcp.Auth;

public class AuthException : Exception
{
    public int? StatusCode { get; }

    public AuthException(string message, int? statusCode = null)
        : base(message)
    {
        StatusCode = statusCode;
    }
}

public class AuthClient
{
    private const int MaxRetries = 3;
    private const int RetryBackoffSeconds = 2;

    private readonly string _dashboardUrl = "http://localhost:3000";
    private readonly string _apiKey;
    private readonly string _sandboxId;
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthClient> _logger;
    private string? _sessionToken;

    public string? SessionToken => _sessionToken;

    /// <summary>The dashboard's MCP streamable-HTTP endpoint.</summary>
    public string ProxyUrl => $"{_dashboardUrl}/api/mcp";

    /// <summary>Headers for ProxyClient with Bearer auth.</summary>
    public Dictionary<string, string> ProxyHeaders
    {
        get
        {
            if (_sessionToken is null)
                throw new AuthException("Not authenticated. Call AuthenticateAsync() first.");
            return new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {_sessionToken}"
            };
        }
    }

    public AuthClient(string apiKey, string sandboxId, HttpClient httpClient, ILogger<AuthClient> logger)
    {
        _apiKey = apiKey;
        _sandboxId = sandboxId;
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _logger = logger;
    }

    /// <summary>
    /// Authenticate with the dashboard and obtain a session token.
    /// Retries up to <see cref="MaxRetries"/> times on transient failures.
    /// </summary>
    public async Task AuthenticateAsync(CancellationToken cancellationToken = default)
    {
        var url = $"{_dashboardUrl}/api/user/auth";
        var payload = new { api_key = _apiKey, sandbox_id = _sandboxId };

        AuthException? lastError = null;

        for (var attempt = 1; attempt <= MaxRetries; attempt++)
        {
            _logger.LogInformation("Authenticating with dashboard at {Url} (attempt {Attempt}/{MaxRetries})",
                url, attempt, MaxRetries);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsJsonAsync(url, payload, cancellationToken);
            }
            catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                lastError = new AuthException(
                    $"Cannot reach dashboard at {_dashboardUrl}. Check DASHBOARD_URL and network connectivity.");
                await BackoffAsync(attempt, cancellationToken);
                continue;
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                lastError = new AuthException("Dashboard authentication request timed out.");
                await BackoffAsync(attempt, cancellationToken);
                continue;
            }
            catch (HttpRequestException ex)
            {
                lastError = new AuthException($"Request failed: {ex.Message}");
                await BackoffAsync(attempt, cancellationToken);
                continue;
            }

            JsonElement data;
            try
            {
                data = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
            }
            catch (JsonException)
            {
                lastError = new AuthException(
                    $"Dashboard returned non-JSON response (HTTP {(int)response.StatusCode}).");
                await BackoffAsync(attempt, cancellationToken);
                continue;
            }

            var statusCode = (int)response.StatusCode;
            if (statusCode != 200)
            {
                var detail = data.TryGetProperty("detail", out var detailProp)
                    ? detailProp.GetString() ?? "Unknown error"
                    : "Unknown error";
                _logger.LogError("Authentication failed [{StatusCode}]: {Detail}", statusCode, detail);

                // Don't retry on client errors (4xx) — these won't resolve themselves
                if (statusCode >= 400 && statusCode < 500)
                    throw new AuthException(detail, statusCode);

                lastError = new AuthException(detail, statusCode);
                await BackoffAsync(attempt, cancellationToken);
                continue;
            }

            var sessionToken = data.TryGetProperty("session_token", out var tokenProp)
                ? tokenProp.GetString()
                : null;

            if (string.IsNullOrEmpty(sessionToken))
            {
                throw new AuthException(
                    "Dashboard returned success but no session_token. Ensure the dashboard is up to date.");
            }

            _sessionToken = sessionToken;
            _logger.LogInformation("Authenticated successfully. Session token obtained.");
            return;
        }

        throw lastError!;
    }

    protected virtual async Task BackoffAsync(int attempt, CancellationToken cancellationToken)
    {
        if (attempt < MaxRetries)
        {
            var delay = RetryBackoffSeconds * (1 << (attempt - 1));
            _logger.LogWarning("Retrying in {Delay}s...", delay);
            await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
        }
    }

    public override string ToString()
    {
        var maskedKey = _apiKey.Length > 16
            ? $"{_apiKey[..12]}...{_apiKey[^4..]}"
            : "***";
        return $"AuthClient(dashboard=\"{_dashboardUrl}\", sandbox=\"{_sandboxId}\", key={maskedKey})";
    }
}
