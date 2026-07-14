using Microsoft.AspNetCore.Authentication;
using Platform.Api.Authentication;
using Platform.Api.Endpoints;
using Platform.Api.Errors;
using Platform.Application.Identity;
using Platform.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddPlatformInfrastructure(builder.Configuration);
builder.Services.AddScoped<UserProfileService>();
builder.Services
    .AddAuthentication(FirebaseAuthenticationDefaults.Scheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(
        FirebaseAuthenticationDefaults.Scheme,
        _ => { });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health/live", () => Results.Ok(new { status = "ok" }))
    .AllowAnonymous();
app.MapOpenApi("/openapi/{documentName}.json");
app.MapIdentityEndpoints();

app.Run();

public partial class Program
{
}
