using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Interfaces;
using System.Data;

namespace ProductRepairDataAccess.DataAccess;

public class DataAccess : IDataAccess
{
    private IConfigurationSettings _configurationSettings;
    private string _connectionString;
    public DataAccess(IConfigurationSettings configurationSettings)
    {
        _configurationSettings = configurationSettings;
        _connectionString = configurationSettings.GetConnectionString();

    }
    public IEnumerable<T> LoadRecord<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            IEnumerable<T> record = connection.Query<T>(sqlStatement, parameters);

            return record;
        }
    }

    public void SaveData<T>(string sqlStatement, T parameters
        )
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(sqlStatement, parameters);
        }
    }
}
