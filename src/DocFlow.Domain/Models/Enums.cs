namespace DocFlow.Domain.Models
{
    public enum UserRole
    {
        Admin,
        User,
        Guest
    }

    public enum DocumentStatus
    {
        Draft,
        InReview,
        Approved,
        Rejected
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }
}