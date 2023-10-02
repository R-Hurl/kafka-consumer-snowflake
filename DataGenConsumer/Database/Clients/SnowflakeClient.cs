using DataGenConsumer.Database.Configs;
using Microsoft.Extensions.Options;
using Snowflake.Data.Client;

namespace DataGenConsumer.Database.Clients
{
    /// <summary>
    /// A client for executing SQL statements against a Snowflake database.
    /// </summary>
    public class SnowflakeClient
    {
        private readonly SnowflakeDbConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeClient"/> class with the specified configuration.
        /// </summary>
        /// <param name="config">The Snowflake database configuration.</param>
        public SnowflakeClient(IOptions<SnowflakeDbConfig> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// Executes a non-query SQL statement against the Snowflake database with the specified parameters.
        /// </summary>
        /// <param name="sql">The SQL statement to execute.</param>
        /// <param name="parameters">The parameters to pass to the SQL statement.</param>
        /// <returns>The number of rows affected by the SQL statement.</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params SnowflakeDbParameter[] parameters)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {              
                throw;
            }
        }

        /// <summary>
        /// Gets a new Snowflake database connection.
        /// </summary>
        /// <returns>A new Snowflake database connection.</returns>
        private async Task<SnowflakeDbConnection> GetConnectionAsync()
        {
            var connection = new SnowflakeDbConnection(_config.ConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}