namespace Platform.Application.Identity;

public sealed record UserProfileDto(
    Guid Id,
    string? Email,
    string? DisplayName,
    string Locale,
    string TimeZoneId,
    Guid? CityId,
    string Status,
    uint Version,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record UpdateUserProfileRequest(
    string? DisplayName,
    string Locale,
    string TimeZoneId,
    Guid? CityId);
