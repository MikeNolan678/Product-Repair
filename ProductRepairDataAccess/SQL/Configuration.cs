using Microsoft.Extensions.Configuration;
using ProductRepairDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.SQL
{
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
}
