namespace Platform.Domain.Identity;

public enum UserAccountStatus : short
{
    Active = 1,
    Restricted = 2,
    Suspended = 3,
    DeletionPending = 4,
    Deleted = 5,
}
