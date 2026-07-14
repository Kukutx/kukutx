using System.Security.Claims;
using Platform.Application.Identity;

namespace Platform.Api.Endpoints;

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1").RequireAuthorization();

        group.MapGet("/me", GetCurrentUserAsync)
            .WithName("GetCurrentUser")
            .Produces<UserProfileDto>();

        group.MapPut("/me", UpdateCurrentUserAsync)
            .WithName("UpdateCurrentUser")
            .Produces<UserProfileDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status428PreconditionRequired);

        return endpoints;
    }

    private static async Task<IResult> GetCurrentUserAsync(
        ClaimsPrincipal principal,
        UserProfileService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var profile = await service.GetOrCreateAsync(ToIdentity(principal), cancellationToken);
        context.Response.Headers.ETag = $"\"{profile.Version}\"";
        return Results.Ok(profile);
    }

    private static async Task<IResult> UpdateCurrentUserAsync(
        ClaimsPrincipal principal,
        UpdateUserProfileRequest request,
        UserProfileService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var rawIfMatch = context.Request.Headers.IfMatch.ToString().Trim();
        if (rawIfMatch.StartsWith('W'))
        {
            rawIfMatch = rawIfMatch[1..].Trim();
        }

        if (!uint.TryParse(rawIfMatch.Trim('"'), out var expectedVersion))
        {
            return Results.Problem(
                statusCode: StatusCodes.Status428PreconditionRequired,
                title: "缺少并发版本",
                detail: "更新资料必须提供有效的 If-Match ETag。",
                extensions: new Dictionary<string, object?>
                {
                    ["code"] = "if_match_required",
                    ["traceId"] = context.TraceIdentifier,
                });
        }

        var profile = await service.UpdateAsync(
            ToIdentity(principal),
            request,
            expectedVersion,
            cancellationToken);
        context.Response.Headers.ETag = $"\"{profile.Version}\"";
        return Results.Ok(profile);
    }

    private static AuthenticatedIdentity ToIdentity(ClaimsPrincipal principal)
    {
        var firebaseUid = principal.FindFirstValue("firebase_uid")
            ?? throw new InvalidOperationException("认证主体缺少 firebase_uid Claim。");
        return new AuthenticatedIdentity(firebaseUid, principal.FindFirstValue(ClaimTypes.Email));
    }
}
