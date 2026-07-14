namespace Platform.Domain.Identity;

public sealed class UserAccount
{
    private UserAccount()
    {
    }

    public Guid Id { get; private set; }

    public string FirebaseUid { get; private set; } = string.Empty;

    public string? Email { get; private set; }

    public string? DisplayName { get; private set; }

    public string Locale { get; private set; } = "zh-Hans";

    public string TimeZoneId { get; private set; } = "UTC";

    public Guid? CityId { get; private set; }

    public UserAccountStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public uint Version { get; private set; }

    public static UserAccount Create(string firebaseUid, string? email, DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firebaseUid);

        return new UserAccount
        {
            Id = Guid.NewGuid(),
            FirebaseUid = firebaseUid.Trim(),
            Email = NormalizeEmail(email),
            Locale = "zh-Hans",
            TimeZoneId = "UTC",
            Status = UserAccountStatus.Active,
            CreatedAt = now,
            UpdatedAt = now,
        };
    }

    public void UpdateProfile(
        string? displayName,
        string locale,
        string timeZoneId,
        Guid? cityId,
        DateTimeOffset now)
    {
        if (Status is UserAccountStatus.Suspended or UserAccountStatus.Deleted)
        {
            throw new InvalidOperationException("当前账户状态不允许更新资料。");
        }

        DisplayName = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim();
        Locale = locale;
        TimeZoneId = timeZoneId;
        CityId = cityId;
        UpdatedAt = now;
    }

    private static string? NormalizeEmail(string? email)
    {
        return string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
    }
}
