using Microsoft.EntityFrameworkCore;
using Platform.Application.Identity;
using Platform.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace Platform.IntegrationTests;

public sealed class UserAccountStoreTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder("postgres:18-alpine")
        .WithDatabase("platform_tests")
        .WithUsername("platform")
        .WithPassword("platform_tests")
        .Build();

    public async Task InitializeAsync()
    {
        await postgres.StartAsync();
        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public Task DisposeAsync()
    {
        return postgres.DisposeAsync().AsTask();
    }

    [Fact]
    public async Task GetOrCreateIsIdempotentAcrossContexts()
    {
        var identity = new AuthenticatedIdentity("firebase-integration-1", "person@example.com");
        Guid firstId;

        await using (var firstContext = CreateContext())
        {
            var store = new EfUserAccountStore(firstContext);
            var first = await store.GetOrCreateAsync(identity, DateTimeOffset.UtcNow, CancellationToken.None);
            firstId = first.Id;
        }

        await using (var secondContext = CreateContext())
        {
            var store = new EfUserAccountStore(secondContext);
            var second = await store.GetOrCreateAsync(identity, DateTimeOffset.UtcNow, CancellationToken.None);
            Assert.Equal(firstId, second.Id);
            Assert.Equal(1, await secondContext.UserAccounts.CountAsync());
        }
    }

    private PlatformDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PlatformDbContext>()
            .UseNpgsql(postgres.GetConnectionString())
            .Options;
        return new PlatformDbContext(options);
    }
}
