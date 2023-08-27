using Microsoft.Extensions.Configuration;
using ProductRepairDataAccess.Models;

namespace ProductRepairDataAccess.DataAccess;

public static class Configuration
{
    private static readonly SQLConnectionConfig _connectionString = new SQLConnectionConfig();

    public static SQLConnectionConfig GetConfigurationSettings(IConfiguration configuration)
    {
        SQLConnectionConfig sqlConnectionConfig = new SQLConnectionConfig();

        configuration.GetSection("ConnectionStrings").Bind(_connectionString);
        sqlConnectionConfig.DbConnection = _connectionString.DbConnection;

        return sqlConnectionConfig;
    }
}
