namespace Platform.Application.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
