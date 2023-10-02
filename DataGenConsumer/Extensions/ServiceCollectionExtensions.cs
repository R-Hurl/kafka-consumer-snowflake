using Confluent.Kafka;
using DataGenConsumer.Database.Clients;
using DataGenConsumer.Database.Configs;
using DataGenConsumer.Database.Repositories;
using DataGenConsumer.Database.Repositories.Interfaces;

namespace DataGenConsumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatagenKafkaConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        var consumerConfig = new ConsumerConfig();
        configuration.GetSection("ConsumerConfig").Bind(consumerConfig);
        services.AddSingleton(consumerConfig);
        services.AddHostedService<DatagenKafkaConsumer>();

        return services;
    }

    public static IServiceCollection AddSnowflakeDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SnowflakeDbConfig>(configuration.GetSection("SnowflakeDbConfig"));
        services.AddScoped<SnowflakeClient>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}