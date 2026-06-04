namespace LuckyBid.Domain.Enums;

public enum UserRole
{
    Buyer,
    Merchant,
    Admin
}

public enum UserStatus
{
    Pending,
    Active,
    Suspended
}

public enum ListingStatus
{
    Draft,
    Active,
    Filled,
    Completed,
    Cancelled
}

public enum TransactionStatus
{
    Pending,
    Success,
    Failed
}
