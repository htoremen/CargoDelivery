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

        queueConfiguration.Names.Add(QueueName.CargoSaga, "Cargo." + QueueName.CargoSaga.ToString()); // Saga
        queueConfiguration.Names.Add(QueueName.CreateCargo, "Cargo." + QueueName.CreateCargo.ToString());
        queueConfiguration.Names.Add(QueueName.CreateSelfie, "Cargo." + QueueName.CreateSelfie.ToString());
        queueConfiguration.Names.Add(QueueName.CargoSendApproved, "Cargo." + QueueName.CargoSendApproved.ToString());
        queueConfiguration.Names.Add(QueueName.CargoApproved, "Cargo." + QueueName.CargoApproved.ToString());
        queueConfiguration.Names.Add(QueueName.CargoRejected, "Cargo." + QueueName.CargoRejected.ToString());


        queueConfiguration.Names.Add(QueueName.RouteSaga, "Route." + QueueName.RouteSaga.ToString()); // Saga
        queueConfiguration.Names.Add(QueueName.RouteConfirmed, "Route." + QueueName.RouteConfirmed.ToString());
        queueConfiguration.Names.Add(QueueName.AutoRoute, "Route." + QueueName.AutoRoute.ToString());
        queueConfiguration.Names.Add(QueueName.ManuelRoute, "Route." + QueueName.ManuelRoute.ToString());
        queueConfiguration.Names.Add(QueueName.RouteCreated, "Route." + QueueName.RouteCreated.ToString());


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