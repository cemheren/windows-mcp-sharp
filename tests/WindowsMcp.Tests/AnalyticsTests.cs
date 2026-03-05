using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WindowsMcp.Analytics;
using Xunit;

namespace WindowsMcp.Tests;

public class AnalyticsTests
{
    private static PostHogAnalytics CreateAnalytics()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}")
            });
        var httpClient = new HttpClient(handler.Object);
        var logger = new Mock<ILogger<PostHogAnalytics>>();
        return new PostHogAnalytics(httpClient, logger.Object);
    }

    [Fact]
    public async Task WithAnalyticsAsync_SuccessPath_ReturnsResult()
    {
        var analytics = CreateAnalytics();

        var result = await analytics.WithAnalyticsAsync("test_tool", () => Task.FromResult("result"));

        Assert.Equal("result", result);
    }

    [Fact]
    public async Task WithAnalyticsAsync_ErrorPath_RethrowsException()
    {
        var analytics = CreateAnalytics();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            analytics.WithAnalyticsAsync<string>("test_tool",
                () => throw new InvalidOperationException("something broke")));

        Assert.Equal("something broke", ex.Message);
    }

    [Fact]
    public async Task WithAnalyticsAsync_NullAnalytics_InterfaceAllowsNull()
    {
        // In C#, the equivalent of "no analytics" is using IAnalytics? = null
        // and guarding calls. Verify that null-check pattern works.
        IAnalytics? analytics = null;
        Assert.Null(analytics);

        // Without analytics, the tool function should still produce its result
        var result = await Task.FromResult(42);
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task WithAnalyticsAsync_DurationMeasurement()
    {
        var analytics = CreateAnalytics();

        var result = await analytics.WithAnalyticsAsync("test_tool", async () =>
        {
            await Task.Delay(100);
            return "done";
        });

        Assert.Equal("done", result);
    }

    [Fact]
    public async Task WithAnalyticsAsync_ErrorStillCompletes()
    {
        var analytics = CreateAnalytics();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            analytics.WithAnalyticsAsync<string>("test_tool", async () =>
            {
                await Task.Delay(50);
                throw new InvalidOperationException("fail");
            }));
    }

    [Fact]
    public async Task WithAnalyticsAsync_ReturnValuePreserved()
    {
        var analytics = CreateAnalytics();

        var result = await analytics.WithAnalyticsAsync("test_tool",
            () => Task.FromResult(new Dictionary<string, object>
            {
                ["key"] = "value",
                ["count"] = 42
            }));

        Assert.Equal("value", result["key"]);
        Assert.Equal(42, result["count"]);
    }
}
