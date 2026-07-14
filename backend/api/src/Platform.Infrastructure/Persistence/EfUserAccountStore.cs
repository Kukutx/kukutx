using Microsoft.EntityFrameworkCore;
using Npgsql;
using Platform.Application.Common;
using Platform.Application.Identity;
using Platform.Domain.Identity;

namespace Platform.Infrastructure.Persistence;

public sealed class EfUserAccountStore(PlatformDbContext dbContext) : IUserAccountStore
{
    public async Task<UserAccount> GetOrCreateAsync(
        AuthenticatedIdentity identity,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var existing = await dbContext.UserAccounts
            .SingleOrDefaultAsync(
                account => account.FirebaseUid == identity.FirebaseUid,
                cancellationToken);

        if (existing is not null)
        {
            return existing;
        }

        var account = UserAccount.Create(identity.FirebaseUid, identity.Email, now);
        dbContext.UserAccounts.Add(account);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return account;
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
            })
        {
            dbContext.ChangeTracker.Clear();
            return await dbContext.UserAccounts.SingleAsync(
                candidate => candidate.FirebaseUid == identity.FirebaseUid,
                cancellationToken);
        }
    }

    public Task<UserAccount?> FindByFirebaseUidAsync(
        string firebaseUid,
        CancellationToken cancellationToken)
    {
        return dbContext.UserAccounts.SingleOrDefaultAsync(
            account => account.FirebaseUid == firebaseUid,
            cancellationToken);
    }

    public async Task SaveAsync(
        UserAccount account,
        uint expectedVersion,
        CancellationToken cancellationToken)
    {
        dbContext.Entry(account).Property(candidate => candidate.Version).OriginalValue = expectedVersion;

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyConflictException();
        }
    }
}
