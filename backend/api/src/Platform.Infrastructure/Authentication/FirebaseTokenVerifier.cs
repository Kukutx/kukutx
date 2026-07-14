using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Platform.Application.Identity;

namespace Platform.Infrastructure.Authentication;

public sealed class FirebaseTokenVerifier(IConfiguration configuration) : IIdentityTokenVerifier
{
    private readonly string projectId = configuration["Authentication:FirebaseProjectId"]
        ?? throw new InvalidOperationException("缺少 Authentication:FirebaseProjectId 配置。");

    private readonly Lazy<FirebaseAuth> firebaseAuth = new(() => CreateFirebaseAuth(configuration));

    public async Task<AuthenticatedIdentity> VerifyAsync(
        string idToken,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var decoded = await firebaseAuth.Value.VerifyIdTokenAsync(idToken, checkRevoked: true);

        var email = decoded.Claims.TryGetValue("email", out var rawEmail)
            ? rawEmail?.ToString()
            : null;

        return new AuthenticatedIdentity(decoded.Uid, email);
    }

    private static FirebaseAuth CreateFirebaseAuth(IConfiguration configuration)
    {
        var projectId = configuration["Authentication:FirebaseProjectId"]
            ?? throw new InvalidOperationException("缺少 Authentication:FirebaseProjectId 配置。");

        var options = new AppOptions
        {
            ProjectId = projectId,
        };

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FIREBASE_AUTH_EMULATOR_HOST")))
        {
            options.Credential = GoogleCredential.GetApplicationDefault();
        }

        var app = FirebaseApp.Create(options, $"platform-{Guid.NewGuid():N}");
        return FirebaseAuth.GetAuth(app);
    }
}
