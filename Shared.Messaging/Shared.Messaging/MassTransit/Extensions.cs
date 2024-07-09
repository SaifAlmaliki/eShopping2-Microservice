using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.MassTransit;
public static class Extentions
{
    // Extension method for IServiceCollection to add a message broker (RabbitMQ in this case)
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        // Adding MassTransit to the service collection
        services.AddMassTransit(config =>
        {
            // Setting endpoint names to be in kebab-case
            config.SetKebabCaseEndpointNameFormatter();

            // If an assembly is provided, add consumers from that assembly
            if (assembly is not null)
                config.AddConsumers(assembly);

            // Configuring MassTransit to use RabbitMQ
            config.UsingRabbitMq((context, configurator) =>
            {
                // Configuring the RabbitMQ host using settings from the configuration
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    // Setting the RabbitMQ username from the configuration
                    host.Username(configuration["MessageBroker:UserName"]);
                    // Setting the RabbitMQ password from the configuration
                    host.Password(configuration["MessageBroker:Password"]);
                });

                // Configuring endpoints for the consumers
                configurator.ConfigureEndpoints(context);
            });
        });

        // Returning the service collection to allow chaining of service configuration
        return services;
    }
}
