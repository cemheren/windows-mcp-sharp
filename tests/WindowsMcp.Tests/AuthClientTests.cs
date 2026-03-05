using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WindowsMcp.Auth;
using Xunit;

namespace WindowsMcp.Tests;

/// <summary>
/// Subclass that skips real backoff delays during tests.
/// </summary>
file class TestableAuthClient : AuthClient
{
    public int BackoffCallCount { get; private set; }
    public List<int> BackoffAttempts { get; } = [];

    public TestableAuthClient(string apiKey, string sandboxId, HttpClient httpClient, ILogger<AuthClient> logger)
        : base(apiKey, sandboxId, httpClient, logger)
    {
    }

    protected override Task BackoffAsync(int attempt, CancellationToken cancellationToken)
    {
        BackoffCallCount++;
        BackoffAttempts.Add(attempt);
        return Task.CompletedTask;
    }
}

public class AuthClientTests
{
    private static readonly Mock<ILogger<AuthClient>> LoggerMock = new();

    private static Mock<HttpMessageHandler> CreateHandler(HttpResponseMessage response)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        return handler;
    }

    private static Mock<HttpMessageHandler> CreateHandler(Exception exception)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(exception);
        return handler;
    }

    // ── AuthException ────────────────────────────

    [Fact]
    public void AuthException_Message()
    {
        var err = new AuthException("something failed");
        Assert.Equal("something failed", err.Message);
        Assert.Null(err.StatusCode);
    }

    [Fact]
    public void AuthException_WithStatusCode()
    {
        var err = new AuthException("unauthorized", statusCode: 401);
        Assert.Equal(401, err.StatusCode);
    }

    // ── Properties ───────────────────────────────

    [Fact]
    public void ProxyUrl_EndsWithApiMcp()
    {
        var client = new AuthClient("key", "sb-1", new HttpClient(), LoggerMock.Object);
        Assert.EndsWith("/api/mcp", client.ProxyUrl);
    }

    [Fact]
    public void ProxyHeaders_BeforeAuth_Throws()
    {
        var client = new AuthClient("key", "sb-1", new HttpClient(), LoggerMock.Object);
        var ex = Assert.Throws<AuthException>(() => _ = client.ProxyHeaders);
        Assert.Contains("Not authenticated", ex.Message);
    }

    [Fact]
    public async Task ProxyHeaders_AfterAuth_ReturnsBearerToken()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { session_token = "tok-123" })
        };
        var handler = CreateHandler(response);
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        await client.AuthenticateAsync();
        var headers = client.ProxyHeaders;

        Assert.Equal("Bearer tok-123", headers["Authorization"]);
    }

    [Fact]
    public void SessionToken_InitiallyNull()
    {
        var client = new AuthClient("key", "sb-1", new HttpClient(), LoggerMock.Object);
        Assert.Null(client.SessionToken);
    }

    [Fact]
    public void ToString_MasksLongKey()
    {
        var client = new AuthClient("sk-wmcp-abcdefghijklmnopqrst", "sb-1", new HttpClient(), LoggerMock.Object);
        var r = client.ToString();
        Assert.Contains("sk-wmcp-abcd", r);
        Assert.Contains("qrst", r);
        Assert.DoesNotContain("abcdefghijklmnopqrst", r);
    }

    [Fact]
    public void ToString_ShortKey_ShowsStars()
    {
        var client = new AuthClient("short", "sb-1", new HttpClient(), LoggerMock.Object);
        var r = client.ToString();
        Assert.Contains("***", r);
    }

    // ── AuthenticateAsync ────────────────────────

    [Fact]
    public async Task Authenticate_Success()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { session_token = "tok-abc" })
        };
        var handler = CreateHandler(response);
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        await client.AuthenticateAsync();

        Assert.Equal("tok-abc", client.SessionToken);
        handler.Protected().Verify(
            "SendAsync", Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
        Assert.Equal(0, client.BackoffCallCount);
    }

    [Fact]
    public async Task Authenticate_ConnectionError_Retries()
    {
        var exception = new HttpRequestException("conn", new SocketException());
        var handler = CreateHandler(exception);
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        var ex = await Assert.ThrowsAsync<AuthException>(() => client.AuthenticateAsync());
        Assert.Contains("Cannot reach dashboard", ex.Message);

        handler.Protected().Verify(
            "SendAsync", Times.Exactly(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Authenticate_Timeout_Retries()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        var ex = await Assert.ThrowsAsync<AuthException>(() => client.AuthenticateAsync());
        Assert.Contains("timed out", ex.Message);

        handler.Protected().Verify(
            "SendAsync", Times.Exactly(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Authenticate_NonJsonResponse_Retries()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("not json")
            });
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        var ex = await Assert.ThrowsAsync<AuthException>(() => client.AuthenticateAsync());
        Assert.Contains("non-JSON response", ex.Message);

        handler.Protected().Verify(
            "SendAsync", Times.Exactly(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Authenticate_4xx_NoRetry()
    {
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = JsonContent.Create(new { detail = "Invalid API key" })
        };
        var handler = CreateHandler(response);
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        var ex = await Assert.ThrowsAsync<AuthException>(() => client.AuthenticateAsync());
        Assert.Contains("Invalid API key", ex.Message);
        Assert.Equal(401, ex.StatusCode);

        handler.Protected().Verify(
            "SendAsync", Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Authenticate_5xx_Retries()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = JsonContent.Create(new { detail = "Internal error" })
            });
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        var ex = await Assert.ThrowsAsync<AuthException>(() => client.AuthenticateAsync());
        Assert.Contains("Internal error", ex.Message);
        Assert.Equal(500, ex.StatusCode);

        handler.Protected().Verify(
            "SendAsync", Times.Exactly(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Authenticate_MissingSessionToken_Throws()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { status = "ok" })
        };
        var handler = CreateHandler(response);
        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        var ex = await Assert.ThrowsAsync<AuthException>(() => client.AuthenticateAsync());
        Assert.Contains("no session_token", ex.Message);
    }

    [Fact]
    public async Task Authenticate_RetryThenSuccess()
    {
        var successResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { session_token = "tok-retry" })
        };
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("conn", new SocketException()))
            .ReturnsAsync(successResponse);

        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);
        await client.AuthenticateAsync();

        Assert.Equal("tok-retry", client.SessionToken);
        handler.Protected().Verify(
            "SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Authenticate_BackoffTiming()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());

        var client = new TestableAuthClient("key", "sb-1", new HttpClient(handler.Object), LoggerMock.Object);

        await Assert.ThrowsAsync<AuthException>(() => client.AuthenticateAsync());

        // MaxRetries = 3; BackoffAsync is called after every attempt (3 times),
        // but only actually delays for the first 2 (when attempt < MaxRetries)
        Assert.Equal(3, client.BackoffCallCount);
        Assert.Contains(1, client.BackoffAttempts);
        Assert.Contains(2, client.BackoffAttempts);
        Assert.Contains(3, client.BackoffAttempts);
    }
}
