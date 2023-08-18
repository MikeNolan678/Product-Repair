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
        private static readonly SQLConnectionConfigModel _connectionString = new SQLConnectionConfigModel();

        public static SQLConnectionConfigModel GetConfigurationSettings(IConfiguration configuration)
        {
            SQLConnectionConfigModel sqlConnectionConfig = new SQLConnectionConfigModel();

            configuration.GetSection("ConnectionStrings").Bind(_connectionString);
            sqlConnectionConfig.DbConnection = _connectionString.DbConnection;

            return sqlConnectionConfig;
        }
    }
}
