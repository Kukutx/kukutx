using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Platform.Application.Identity;

namespace Platform.Api.Authentication;

public static class FirebaseAuthenticationDefaults
{
    public const string Scheme = "Firebase";
}

public sealed class FirebaseAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IIdentityTokenVerifier tokenVerifier)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();
        if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        var token = authorization["Bearer ".Length..].Trim();
        if (token.Length == 0)
        {
            return AuthenticateResult.Fail("Bearer Token 不能为空。");
        }

        try
        {
            var identity = await tokenVerifier.VerifyAsync(token, Context.RequestAborted);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, identity.FirebaseUid),
                new("firebase_uid", identity.FirebaseUid),
            };

            if (!string.IsNullOrWhiteSpace(identity.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, identity.Email));
            }

            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(claimsIdentity);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            Logger.LogWarning("Firebase Token 验证失败：{ExceptionType}", exception.GetType().Name);
            return AuthenticateResult.Fail("Token 无效、已过期或已撤销。");
        }
    }
}
