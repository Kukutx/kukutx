namespace Platform.Application.Common;

public sealed class RequestValidationException(IReadOnlyDictionary<string, string[]> errors)
    : Exception("请求验证失败。")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}

public sealed class ConcurrencyConflictException()
    : Exception("资源已被其他请求修改。请刷新后重试。");

public sealed class ResourceNotFoundException(string resourceName)
    : Exception($"未找到资源：{resourceName}。");
