namespace Core.Domain.Enums;

public enum QueueState
{
    None = 0,

    // Zimmetine Geçir
    CreateOrder = 101,
    CreateSelfie = 102,
    OrderApproved = 103,
    OrderRejected = 104,

    // Güzergah Oluştur
    RouteConfirmed = 201,
    AutoRoute = 202,
    ManuelRoute = 203,
    RouteCreated = 204,

    // Kargo İşlemleri

    // Teslim Edilemedi
    NotDelivered = 301,
    CreateRefund = 302,
    TakeSelfei = 303,
    CargoRefundCompleted = 304,

    // İade Oluştur
    SendDamageReport = 311,
    DamageRecordCompletion = 312,

    // Teslimat Oluştur
    CreateDelivery = 321,
    // Kredi Kartı
    CardPayment = 331,
    CardPaymentCompleted = 332,
    // Manuel Ödeme
    PayAtDoor = 341,
    PayAtDoorCompleted = 342,
    // Teslimat İptal
    RollBack = 351,
    CancelDelivered = 352,

    // Kargo İşlemleri Ortak
    DisributionCheck = 361,
    ShiftCompletion = 362,
}
