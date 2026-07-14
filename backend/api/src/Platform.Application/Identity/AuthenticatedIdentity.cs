namespace Platform.Application.Identity;

public sealed record AuthenticatedIdentity(string FirebaseUid, string? Email);
