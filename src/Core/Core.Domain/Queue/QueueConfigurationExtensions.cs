using Microsoft.Extensions.DependencyInjection;
using Core.Domain;

public static class QueueConfigurationExtensions
{
    public static IServiceCollection AddQueueConfiguration(this IServiceCollection services, out IQueueConfiguration queueConfiguration)
    {
        queueConfiguration = new QueueConfiguration()
        {
            Names = new Dictionary<QueueName, string>()
        };

        queueConfiguration.Names.Add(QueueName.CargoSaga, "cargo.saga");

        queueConfiguration.Names.Add(QueueName.CreateCargo, "create.cargo");
        queueConfiguration.Names.Add(QueueName.CreateSelfie, "create.selfie");
        queueConfiguration.Names.Add(QueueName.CargoSendApproved, "cargo.send.approved");
        queueConfiguration.Names.Add(QueueName.CargoApproved, "create.approved");
        queueConfiguration.Names.Add(QueueName.CargoRejected, "cargo.rejected");

        queueConfiguration.Names.Add(QueueName.RouteConfirmed, QueueName.RouteConfirmed.ToString());
        queueConfiguration.Names.Add(QueueName.AutoRoute, QueueName.AutoRoute.ToString());
        queueConfiguration.Names.Add(QueueName.ManuelRoute, QueueName.ManuelRoute.ToString());
        queueConfiguration.Names.Add(QueueName.RouteCreated, QueueName.RouteCreated.ToString());

        queueConfiguration.Names.Add(QueueName.NotDelivered, QueueName.NotDelivered.ToString());
        queueConfiguration.Names.Add(QueueName.CreateRefund, QueueName.CreateRefund.ToString());
        queueConfiguration.Names.Add(QueueName.TakeSelfei, QueueName.TakeSelfei.ToString());
        queueConfiguration.Names.Add(QueueName.CargoRefundCompleted, QueueName.CargoRefundCompleted.ToString());

        queueConfiguration.Names.Add(QueueName.SendDamageReport, QueueName.SendDamageReport.ToString());
        queueConfiguration.Names.Add(QueueName.DamageRecordCompletion, QueueName.DamageRecordCompletion.ToString());

        // Delivery Process
        queueConfiguration.Names.Add(QueueName.CreateDelivery, QueueName.CreateDelivery.ToString());

        queueConfiguration.Names.Add(QueueName.CardPayment, QueueName.CardPayment.ToString());
        queueConfiguration.Names.Add(QueueName.CardPaymentCompleted, QueueName.CardPaymentCompleted.ToString());

        queueConfiguration.Names.Add(QueueName.PayAtDoor, QueueName.PayAtDoor.ToString());
        queueConfiguration.Names.Add(QueueName.PayAtDoorCompleted, QueueName.PayAtDoorCompleted.ToString());

        queueConfiguration.Names.Add(QueueName.RollBack, QueueName.RollBack.ToString());
        queueConfiguration.Names.Add(QueueName.CancelDelivered, QueueName.CancelDelivered.ToString());

        queueConfiguration.Names.Add(QueueName.DisributionCheck, QueueName.DisributionCheck.ToString());
        queueConfiguration.Names.Add(QueueName.ShiftCompletion, QueueName.ShiftCompletion.ToString());

        if(services != null)
            services.AddSingleton<IQueueConfiguration>(queueConfiguration);

        return services;
    }
}