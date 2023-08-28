using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Interfaces;
using System.Data;

namespace ProductRepairDataAccess.DataAccess;

public class DataAccessOperations : IDataAccessOperations
{
    private IConfigurationSettings _configurationSettings;
    private string _connectionString;
    public DataAccessOperations(IConfigurationSettings configurationSettings)
    {
        _configurationSettings = configurationSettings;
        _connectionString = configurationSettings.GetConnectionString();

    }
    public IEnumerable<T> LoadRecords<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            IEnumerable<T> record = connection.Query<T>(sqlStatement, parameters);

            return record;
        }
    }

    public T SaveAndReturnRecord<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            T result = connection.QuerySingleOrDefault<T>(sqlStatement, parameters);

            return result; // Return the Identity
        }
    }

    public void SaveData<T>(string sqlStatement, T parameters)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(sqlStatement, parameters);
        }
    }
}
