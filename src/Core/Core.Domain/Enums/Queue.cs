namespace Core.Domain.Enums;

public enum Queue
{
    None = 0,
    CreateOrder = 1,
    CreateSelfie = 2,
    OrderApproved = 3,
    OrderRejected = 4,

    RouteConfirmed = 5,
    AutoRoute = 6,
    ManuelRoute = 7,
    RouteCreated = 8,

    NotDelivered = 9,
    CreateRefund = 10,
    TakeSelfei = 11,
    CargoRefundCompleted = 12,

    DisributionComplete=13,
}
