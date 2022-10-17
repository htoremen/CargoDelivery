namespace Core.Domain;

public static class StaticKeyValues
{
    public static string CreateDebit { get; set; } = "create-debit-";
    public static string SendSelfie { get; set; } = "send-selfie-";
    public static string CargoApproval { get; set; } = "cargo-approval-";

    public static string AutoRoute { get; set; } = "auto-route-";
    public static string ManuelRoute { get; set; } = "manuel-route-";

    public static string StartDistribution { get; set; } = "start-Distribution-";
    public static string VerificationCode { get; set; } = "verification-code-";
    public static string CreateDelivery { get; set; } = "create-delivery-";
    public static string NotDelivered { get; set; } = "not-delivered-";
    public static string CreateRefund { get; set; } = "create-refund-";
    public static string ShiftCompletion { get; set; } = "shift-completion-";
}
