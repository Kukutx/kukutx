using Platform.Domain.Identity;

namespace Platform.Application.Identity;

public interface IUserAccountStore
{
    Task<UserAccount> GetOrCreateAsync(
        AuthenticatedIdentity identity,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task<UserAccount?> FindByFirebaseUidAsync(string firebaseUid, CancellationToken cancellationToken);

    Task SaveAsync(UserAccount account, uint expectedVersion, CancellationToken cancellationToken);
}
