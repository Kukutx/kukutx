using Platform.Application.Abstractions;
using Platform.Application.Common;
using Platform.Domain.Identity;

namespace Platform.Application.Identity;

public sealed class UserProfileService(IUserAccountStore store, IClock clock)
{
    private static readonly HashSet<string> SupportedLocales =
        ["zh-Hans", "zh-Hant", "en"];

    public async Task<UserProfileDto> GetOrCreateAsync(
        AuthenticatedIdentity identity,
        CancellationToken cancellationToken)
    {
        var account = await store.GetOrCreateAsync(identity, clock.UtcNow, cancellationToken);
        return Map(account);
    }

    public async Task<UserProfileDto> UpdateAsync(
        AuthenticatedIdentity identity,
        UpdateUserProfileRequest request,
        uint expectedVersion,
        CancellationToken cancellationToken)
    {
        Validate(request);

        var account = await store.FindByFirebaseUidAsync(identity.FirebaseUid, cancellationToken)
            ?? throw new ResourceNotFoundException("UserAccount");

        account.UpdateProfile(
            request.DisplayName,
            request.Locale,
            request.TimeZoneId,
            request.CityId,
            clock.UtcNow);

        await store.SaveAsync(account, expectedVersion, cancellationToken);
        return Map(account);
    }

    private static void Validate(UpdateUserProfileRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);

        if (request.DisplayName?.Trim().Length > 80)
        {
            errors[nameof(request.DisplayName)] = ["昵称不能超过 80 个字符。"];
        }

        if (!SupportedLocales.Contains(request.Locale))
        {
            errors[nameof(request.Locale)] = ["语言必须是 zh-Hans、zh-Hant 或 en。"];
        }

        if (string.IsNullOrWhiteSpace(request.TimeZoneId))
        {
            errors[nameof(request.TimeZoneId)] = ["时区不能为空。"];
        }
        else
        {
            try
            {
                _ = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                errors[nameof(request.TimeZoneId)] = ["必须提供有效的 IANA 时区。"];
            }
            catch (InvalidTimeZoneException)
            {
                errors[nameof(request.TimeZoneId)] = ["时区配置无效。"];
            }
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }
    }

    private static UserProfileDto Map(UserAccount account)
    {
        return new UserProfileDto(
            account.Id,
            account.Email,
            account.DisplayName,
            account.Locale,
            account.TimeZoneId,
            account.CityId,
            account.Status.ToString(),
            account.Version,
            account.CreatedAt,
            account.UpdatedAt);
    }
}
