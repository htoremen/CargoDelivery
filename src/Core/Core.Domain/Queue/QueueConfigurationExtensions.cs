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
        queueConfiguration.Names.Add(QueueName.SendSelfie, "Cargo." + QueueName.SendSelfie.ToString());
        queueConfiguration.Names.Add(QueueName.CargoApproval, "Cargo." + QueueName.CargoApproval.ToString());
        queueConfiguration.Names.Add(QueueName.CargoRejected, "Cargo." + QueueName.CargoRejected.ToString());

        // Route
        queueConfiguration.Names.Add(QueueName.StartRoute, "Route." + QueueName.StartRoute.ToString());
        queueConfiguration.Names.Add(QueueName.RouteConfirmed, "Route." + QueueName.RouteConfirmed.ToString());
        queueConfiguration.Names.Add(QueueName.AutoRoute, "Route." + QueueName.AutoRoute.ToString());
        queueConfiguration.Names.Add(QueueName.ManuelRoute, "Route." + QueueName.ManuelRoute.ToString());

        queueConfiguration.Names.Add(QueueName.StartDelivery, "Delivery." + QueueName.StartDelivery.ToString());
        queueConfiguration.Names.Add(QueueName.NotDelivered, "Delivery." + QueueName.NotDelivered.ToString());


        queueConfiguration.Names.Add(QueueName.CreateRefund, "Delivery." + QueueName.CreateRefund.ToString());

        // Delivery Process
        queueConfiguration.Names.Add(QueueName.CreateDelivery, "Delivery." + QueueName.CreateDelivery.ToString());

        queueConfiguration.Names.Add(QueueName.CardPayment, "Payment." + QueueName.CardPayment.ToString());       
        queueConfiguration.Names.Add(QueueName.PayAtDoor, "Payment." + QueueName.PayAtDoor.ToString());        
        queueConfiguration.Names.Add(QueueName.FreeDelivery, "Payment." + QueueName.FreeDelivery.ToString());
        
        queueConfiguration.Names.Add(QueueName.DeliveryCompleted, "Delivery." + QueueName.DeliveryCompleted.ToString());
      
        queueConfiguration.Names.Add(QueueName.ShiftCompletion, "Delivery." + QueueName.ShiftCompletion.ToString());

        if(services != null)
            services.AddSingleton<IQueueConfiguration>(queueConfiguration);

        return services;
    }
}