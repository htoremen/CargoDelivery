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

        // Cargo
        queueConfiguration.Names.Add(QueueName.CargoSaga, "Cargo." + QueueName.CargoSaga.ToString()); // Saga
        queueConfiguration.Names.Add(QueueName.CreateCargo, "Cargo." + QueueName.CreateCargo.ToString());
        queueConfiguration.Names.Add(QueueName.CreateSelfie, "Cargo." + QueueName.CreateSelfie.ToString());
        queueConfiguration.Names.Add(QueueName.CargoApproved, "Cargo." + QueueName.CargoApproved.ToString());
        queueConfiguration.Names.Add(QueueName.CargoRejected, "Cargo." + QueueName.CargoRejected.ToString());

        // Route
        queueConfiguration.Names.Add(QueueName.StartRoute, "Route." + QueueName.StartRoute.ToString());
        queueConfiguration.Names.Add(QueueName.RouteConfirmed, "Route." + QueueName.RouteConfirmed.ToString());
        queueConfiguration.Names.Add(QueueName.AutoRoute, "Route." + QueueName.AutoRoute.ToString());
        queueConfiguration.Names.Add(QueueName.ManuelRoute, "Route." + QueueName.ManuelRoute.ToString());

        queueConfiguration.Names.Add(QueueName.StartDelivery, "Delivery." + QueueName.StartDelivery.ToString());
        queueConfiguration.Names.Add(QueueName.NotDelivered, "Delivery." + QueueName.NotDelivered.ToString());
        queueConfiguration.Names.Add(QueueName.NotDeliveredCreateRefund, "Delivery.NotDelivered." + QueueName.NotDeliveredCreateRefund.ToString());
        queueConfiguration.Names.Add(QueueName.TakeSelfei, "Delivery.NotDelivered." + QueueName.TakeSelfei.ToString());
        queueConfiguration.Names.Add(QueueName.CargoRefundCompleted, "Delivery.NotDelivered." + QueueName.CargoRefundCompleted.ToString());


        queueConfiguration.Names.Add(QueueName.CreateRefund, "Delivery." + QueueName.CreateRefund.ToString());
        queueConfiguration.Names.Add(QueueName.CreateRefundCompletion, "Delivery.CreateRefund." + QueueName.CreateRefundCompletion.ToString());

        // Delivery Process
        queueConfiguration.Names.Add(QueueName.CreateDelivery, "Delivery." + QueueName.CreateDelivery.ToString());

        queueConfiguration.Names.Add(QueueName.CardPayment, "Delivery.Payment." + QueueName.CardPayment.ToString());
        queueConfiguration.Names.Add(QueueName.CardPaymentCompleted, "Delivery.Payment." + QueueName.CardPaymentCompleted.ToString());

        queueConfiguration.Names.Add(QueueName.PayAtDoor, "Delivery." + QueueName.PayAtDoor.ToString());
        queueConfiguration.Names.Add(QueueName.PayAtDoorCompleted, "Delivery." + QueueName.PayAtDoorCompleted.ToString());

        queueConfiguration.Names.Add(QueueName.RollBack, "Delivery." + QueueName.RollBack.ToString());
        queueConfiguration.Names.Add(QueueName.CancelDelivered, "Delivery." + QueueName.CancelDelivered.ToString());

        queueConfiguration.Names.Add(QueueName.DeliveryCompleted, "Delivery." + QueueName.DeliveryCompleted.ToString());
      
        queueConfiguration.Names.Add(QueueName.ShiftCompletion, "Delivery." + QueueName.ShiftCompletion.ToString());

        if(services != null)
            services.AddSingleton<IQueueConfiguration>(queueConfiguration);

        return services;
    }
}