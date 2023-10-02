using DataGenConsumer.Database.Clients;
using DataGenConsumer.Database.Repositories.Interfaces;
using Snowflake.Data.Client;

namespace DataGenConsumer.Database.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly SnowflakeClient _client;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(SnowflakeClient client, ILogger<OrderRepository> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InsertOrderAsync(string orderId, string order)
    {
        var sql = "CALL INSERT_ORDERS(?, ?)";
        var parameters = new[]
        {
            new SnowflakeDbParameter(1, Snowflake.Data.Core.SFDataType.TEXT) { Value = orderId},
            new SnowflakeDbParameter(2, Snowflake.Data.Core.SFDataType.TEXT) { Value = order},
        };

        try
        {
            await _client.ExecuteNonQueryAsync(sql, parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting order", order);
            throw;
        }
        
    }
}
