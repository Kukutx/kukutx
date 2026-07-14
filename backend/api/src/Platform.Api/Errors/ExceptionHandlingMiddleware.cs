using Platform.Application.Common;

namespace Platform.Api.Errors;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (RequestValidationException exception)
        {
            await WriteProblemAsync(
                context,
                StatusCodes.Status400BadRequest,
                "validation_failed",
                "请求验证失败",
                exception.Message,
                new Dictionary<string, object?> { ["errors"] = exception.Errors });
        }
        catch (ResourceNotFoundException exception)
        {
            await WriteProblemAsync(
                context,
                StatusCodes.Status404NotFound,
                "resource_not_found",
                "资源不存在",
                exception.Message);
        }
        catch (ConcurrencyConflictException exception)
        {
            await WriteProblemAsync(
                context,
                StatusCodes.Status409Conflict,
                "concurrency_conflict",
                "资源已更新",
                exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "未处理异常，TraceId={TraceId}", context.TraceIdentifier);
            await WriteProblemAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "internal_error",
                "服务器内部错误",
                "请求未能完成。请稍后重试。");
        }
    }

    private static Task WriteProblemAsync(
        HttpContext context,
        int status,
        string code,
        string title,
        string detail,
        IDictionary<string, object?>? extraExtensions = null)
    {
        var extensions = extraExtensions is null
            ? new Dictionary<string, object?>()
            : new Dictionary<string, object?>(extraExtensions);
        extensions["code"] = code;
        extensions["traceId"] = context.TraceIdentifier;

        return Results.Problem(
            statusCode: status,
            title: title,
            detail: detail,
            extensions: extensions).ExecuteAsync(context);
    }
}
