using Microsoft.Extensions.Configuration;
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.Models;

namespace ProductRepairDataAccess.DataAccess;

public class ConfigurationSettings : IConfigurationSettings
{
    private IConfiguration _configuration;
    public ConfigurationSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString()
    {
        SqlConnectionConfig sqlConnectionConfig = new SqlConnectionConfig();

        _configuration.GetSection("ConnectionStrings").Bind(sqlConnectionConfig);

        return sqlConnectionConfig.DbConnection;
    }
}
