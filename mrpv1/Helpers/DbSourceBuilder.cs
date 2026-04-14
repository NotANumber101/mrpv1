using Npgsql;

namespace mrpv1.Helpers;

public class DbSourceBuilder(string host)
{
    readonly static string loadBalanaceHosts = "Load Balance Hosts=true;";
    readonly static string targetSessionAttributes = "Target Session Attributes=prefer-standby;";
    readonly static string dbName = "mrpv1db";
    readonly string connectionString = $"Host={host};Port=5432;Username=postgres;Password=password;Database={dbName};";

    public NpgsqlDataSourceBuilder Builder()
    {
        // using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        var npgsqlDataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString+loadBalanaceHosts+targetSessionAttributes);
        return npgsqlDataSourceBuilder
            .EnableParameterLogging(true);
            // .UseLoggerFactory(factory);
    }
}