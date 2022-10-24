namespace Core.Domain.Enums;

public enum QueueName
{
    None = 0,
    CargoSaga = 10,

    // Zimmetine Geçir
    CreateDebit = 101,
    CreateCargo = 102,
    SendSelfie = 103,
    DebitApproval = 104,
    CargoRejected = 105,
    CreateDebitHistory = 111,

    CreateDebitFault = 121,

    ShipmentReceived = 130, // Gönderi Alındı
    StartDistribution = 131,
    WasDelivered = 132,


    // Güzergah Oluştur
    StartRoute = 200,
    AutoRoute = 201,
    ManuelRoute = 202,

    // Kargo İşlemleri
    StartDelivery = 300,
    NewDelivery = 301,

    SendMail = 303,
    SendSms = 304,
    PushNotification = 305,
    VerificationCode = 306,

    NotDelivered = 310,
    CreateRefund = 320,

    // Teslimat Oluştur
    CreateDelivery = 331,
    CardPayment = 332,
    PayAtDoor = 333,
    FreeDelivery = 334,

    // Kargo İşlemleri Ortak
    DeliveryCompleted = 361,

    // Vardia Tamamlama
    ShiftCompletion = 371,
}
