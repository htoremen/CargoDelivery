using Microsoft.Extensions.DependencyInjection;
using Core.Domain;

public static class QueueConfigurationExtensions
{
    public static IServiceCollection AddQueueConfiguration(this IServiceCollection services, out IQueueConfiguration queueConfiguration)
    {
        queueConfiguration = new QueueConfiguration()
        {
            Names = new Dictionary<QueueState, string>()
        };

        queueConfiguration.Names.Add(QueueState.CreateCargo, QueueState.CreateCargo.ToString());
        queueConfiguration.Names.Add(QueueState.CreateSelfie, QueueState.CreateSelfie.ToString());
        queueConfiguration.Names.Add(QueueState.CargoApproved, QueueState.CargoApproved.ToString());
        queueConfiguration.Names.Add(QueueState.CargoSendApproved, QueueState.CargoSendApproved.ToString());
        queueConfiguration.Names.Add(QueueState.CargoRejected, QueueState.CargoRejected.ToString());

        queueConfiguration.Names.Add(QueueState.RouteConfirmed, QueueState.RouteConfirmed.ToString());
        queueConfiguration.Names.Add(QueueState.AutoRoute, QueueState.AutoRoute.ToString());
        queueConfiguration.Names.Add(QueueState.ManuelRoute, QueueState.ManuelRoute.ToString());
        queueConfiguration.Names.Add(QueueState.RouteCreated, QueueState.RouteCreated.ToString());

        queueConfiguration.Names.Add(QueueState.NotDelivered, QueueState.NotDelivered.ToString());
        queueConfiguration.Names.Add(QueueState.CreateRefund, QueueState.CreateRefund.ToString());
        queueConfiguration.Names.Add(QueueState.TakeSelfei, QueueState.TakeSelfei.ToString());
        queueConfiguration.Names.Add(QueueState.CargoRefundCompleted, QueueState.CargoRefundCompleted.ToString());

        queueConfiguration.Names.Add(QueueState.SendDamageReport, QueueState.SendDamageReport.ToString());
        queueConfiguration.Names.Add(QueueState.DamageRecordCompletion, QueueState.DamageRecordCompletion.ToString());

        // Delivery Process
        queueConfiguration.Names.Add(QueueState.CreateDelivery, QueueState.CreateDelivery.ToString());

        queueConfiguration.Names.Add(QueueState.CardPayment, QueueState.CardPayment.ToString());
        queueConfiguration.Names.Add(QueueState.CardPaymentCompleted, QueueState.CardPaymentCompleted.ToString());

        queueConfiguration.Names.Add(QueueState.PayAtDoor, QueueState.PayAtDoor.ToString());
        queueConfiguration.Names.Add(QueueState.PayAtDoorCompleted, QueueState.PayAtDoorCompleted.ToString());

        queueConfiguration.Names.Add(QueueState.RollBack, QueueState.RollBack.ToString());
        queueConfiguration.Names.Add(QueueState.CancelDelivered, QueueState.CancelDelivered.ToString());

        queueConfiguration.Names.Add(QueueState.DisributionCheck, QueueState.DisributionCheck.ToString());
        queueConfiguration.Names.Add(QueueState.ShiftCompletion, QueueState.ShiftCompletion.ToString());

        services.AddSingleton<IQueueConfiguration>(queueConfiguration);

        return services;
    }
}