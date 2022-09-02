namespace Core.Domain.Enums;

public enum QueueName
{
    None = 0,
    CargoSaga = 10,

    // Zimmetine Geçir
    CreateCargo = 101,
    SendSelfie = 102,
    CargoApproved = 103,
    CargoRejected = 104,

    // Güzergah Oluştur
    StartRoute = 200,
    RouteConfirmed = 201,
    AutoRoute = 202,
    ManuelRoute = 203,

    // Kargo İşlemleri
    StartDelivery = 300,
    NotDelivered = 301,
    CreateRefund = 311,

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
    DeliveryCompleted = 361,

    // Vardia Tamamlama
    ShiftCompletion = 371,
}
