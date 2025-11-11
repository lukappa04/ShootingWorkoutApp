using System.Security.Claims;
using SWBackend.DataBase;
using SWBackend.Models.Log;

namespace SWBackend.ServiceLayer.Log;

public class UserActionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public UserActionLoggingMiddleware(RequestDelegate next, ILogger<UserActionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext, SwDbContext dbContext)
    {
        _logger?.LogInformation("UserActionLoggingMiddleware called");
        var user = httpContext.User.Identity?.IsAuthenticated == true ? httpContext.User : null;
        
        string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
        string userName = user?.Identity?.Name ?? "Anonymous";
        var log = new UserActionLog
        {
            UserId = userId,
            UserName = userName,
            HttpMethod = httpContext.Request.Method,
            Path = httpContext.Request.Path,
            QueryString = httpContext.Request.QueryString.ToString(),
            ActionName = httpContext.GetEndpoint()?.DisplayName ?? "Unknown",
            RecordDateTime = DateTime.UtcNow
        };
        dbContext.UserActionLogs.Add(log);
        await dbContext.SaveChangesAsync();
        await _next(httpContext);
        _logger?.LogInformation("UserActionLoggingMiddleware ended");
    }
}