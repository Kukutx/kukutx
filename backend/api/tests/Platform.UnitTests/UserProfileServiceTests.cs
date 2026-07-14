using Platform.Application.Abstractions;
using Platform.Application.Common;
using Platform.Application.Identity;
using Platform.Domain.Identity;

namespace Platform.UnitTests;

public sealed class UserProfileServiceTests
{
    [Fact]
    public async Task GetOrCreateReturnsDefaultProfile()
    {
        var now = new DateTimeOffset(2026, 7, 14, 8, 0, 0, TimeSpan.Zero);
        var store = new InMemoryUserAccountStore();
        var service = new UserProfileService(store, new FixedClock(now));

        var result = await service.GetOrCreateAsync(
            new AuthenticatedIdentity("firebase-user-1", "USER@example.com"),
            CancellationToken.None);

        Assert.Equal("user@example.com", result.Email);
        Assert.Equal("zh-Hans", result.Locale);
        Assert.Equal("UTC", result.TimeZoneId);
        Assert.Equal("Active", result.Status);
    }

    [Fact]
    public async Task UpdateRejectsUnsupportedLocale()
    {
        var store = new InMemoryUserAccountStore();
        var service = new UserProfileService(store, new FixedClock(DateTimeOffset.UtcNow));
        var identity = new AuthenticatedIdentity("firebase-user-2", null);
        _ = await service.GetOrCreateAsync(identity, CancellationToken.None);

        await Assert.ThrowsAsync<RequestValidationException>(() => service.UpdateAsync(
            identity,
            new UpdateUserProfileRequest("Test", "fr", "Europe/Rome", null),
            0,
            CancellationToken.None));
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class InMemoryUserAccountStore : IUserAccountStore
    {
        private readonly Dictionary<string, UserAccount> accounts = new(StringComparer.Ordinal);

        public Task<UserAccount> GetOrCreateAsync(
            AuthenticatedIdentity identity,
            DateTimeOffset now,
            CancellationToken cancellationToken)
        {
            if (!accounts.TryGetValue(identity.FirebaseUid, out var account))
            {
                account = UserAccount.Create(identity.FirebaseUid, identity.Email, now);
                accounts.Add(identity.FirebaseUid, account);
            }

            return Task.FromResult(account);
        }

        public Task<UserAccount?> FindByFirebaseUidAsync(
            string firebaseUid,
            CancellationToken cancellationToken)
        {
            accounts.TryGetValue(firebaseUid, out var account);
            return Task.FromResult(account);
        }

        public Task SaveAsync(
            UserAccount account,
            uint expectedVersion,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
