namespace DataGenConsumer.Database.Configs;

public class SnowflakeDbConfig
{
    public string ConnectionString => BuildConnectionString();
    public string? Username => Environment.GetEnvironmentVariable("SNOWFLAKE_USERNAME");
    public string? Password => Environment.GetEnvironmentVariable("SNOWFLAKE_PASSWORD");
    public string? Account => Environment.GetEnvironmentVariable("SNOWFLAKE_ACCOUNT");
    public string Database { get; set; }
    public string Schema { get; set; }

    private string BuildConnectionString()
    {
        return $"account={Account};user={Username};password={Password};db={Database};schema={Schema}";
    }
}