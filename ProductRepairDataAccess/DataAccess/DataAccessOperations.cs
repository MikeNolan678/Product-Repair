using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Interfaces;
using System.Data;

namespace ProductRepairDataAccess.DataAccess;
// TODO: Make DbCallsAsync
public class DataAccessOperations : IDataAccessOperations
{
    private IConfigurationSettings _configurationSettings;
    private string _connectionString;
    public DataAccessOperations(IConfigurationSettings configurationSettings)
    {
        _configurationSettings = configurationSettings;
        _connectionString = configurationSettings.GetConnectionString();

    }
    public async Task<List<T>> LoadRecordsAsync<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
             IEnumerable<T> record = await Task.Run(() => connection.QueryAsync<T>(sqlStatement, parameters));

            return record.ToList();
        }
    }

    public async Task<T> SaveAndReturnRecordAsync<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            T result = await Task.Run(() => connection.QuerySingleOrDefaultAsync<T>(sqlStatement, parameters));

            return result; // Return the Identity
        }
    }

    public async Task SaveDataAsync<T>(string sqlStatement, T parameters)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
           await Task.Run(() => connection.ExecuteAsync(sqlStatement, parameters));
        }
    }
}
