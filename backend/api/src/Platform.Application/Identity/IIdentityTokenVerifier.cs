namespace Platform.Application.Identity;

public interface IIdentityTokenVerifier
{
    Task<AuthenticatedIdentity> VerifyAsync(string idToken, CancellationToken cancellationToken);
}
