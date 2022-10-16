namespace Core.Domain.Enums;

public enum QueueName
{
    None = 0,
    CargoSaga = 10,

    // Zimmetine Geçir
    CreateDebit = 101,
    CreateCargo = 102,
    SendSelfie = 103,
    CargoApproval = 104,
    CargoRejected = 105,
    CreateDebitHistory = 111,

    CreateDebitFault = 121,

    ShipmentReceived = 131, // Gönderi Alındı


    // Güzergah Oluştur
    StartRoute = 200,
    AutoRoute = 201,
    ManuelRoute = 202,

    // Kargo İşlemleri
    StartDelivery = 300,
    NewDelivery = 301,

    SendMail = 302,
    SendSms = 303,
    PushNotification = 304,

    NotDelivered = 310,
    CreateRefund = 320,

    // Teslimat Oluştur
    StartDistribution = 330,
    CreateDelivery = 331,
    CardPayment = 332,
    PayAtDoor = 333,
    FreeDelivery = 334,

    // Kargo İşlemleri Ortak
    DeliveryCompleted = 361,

    // Vardia Tamamlama
    ShiftCompletion = 371,
}
