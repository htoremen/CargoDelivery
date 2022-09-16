namespace Core.Domain.Enums;

public enum QueueName
{
    None = 0,
    CargoSaga = 10,

    // Zimmetine Geçir
    CreateCargo = 101,
    SendSelfie = 102,
    CargoApproval = 103,
    CargoRejected = 104,

    // Güzergah Oluştur
    StartRoute = 200,
    AutoRoute = 201,
    ManuelRoute = 202,

    // Kargo İşlemleri
    StartDelivery = 300,
    NotDelivered = 301,
    CreateRefund = 311,

    // Teslimat Oluştur
    CreateDelivery = 321,
    // Kredi Kartı
    CardPayment = 331,
    // Manuel Ödeme
    PayAtDoor = 341,
    // Teslimat İptal
    FreeDelivery = 351,

    // Kargo İşlemleri Ortak
    DeliveryCompleted = 361,

    // Vardia Tamamlama
    ShiftCompletion = 371,
}
