using DataGenConsumer.Models;

namespace DataGenConsumer.Database.Repositories.Interfaces;


public interface IOrderRepository
{
    /// <summary>
    /// Inserts an order into the Snowflake database with the specified order ID and raw JSON value.
    /// </summary>
    /// <param name="orderId">The identifier for the incoming order.</param>
    /// <param name="order">The raw JSON value for the order.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InsertOrderAsync(string orderId, string order);
}