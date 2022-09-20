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
    NewDelivery = 301,

    NotDelivered = 310,
    CreateRefund = 320,

    // Teslimat Oluştur
    CreateDelivery = 330,
    CardPayment = 331,
    PayAtDoor = 332,
    FreeDelivery = 333,

    // Kargo İşlemleri Ortak
    DeliveryCompleted = 361,

    // Vardia Tamamlama
    ShiftCompletion = 371,
}
