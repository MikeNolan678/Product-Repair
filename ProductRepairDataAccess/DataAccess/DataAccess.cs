using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Interfaces;
using System.Data;

namespace ProductRepairDataAccess.DataAccess;

public class DataAccess : IDataAccess
{
    public IEnumerable<T> LoadRecord<T, U>(string sqlStatement, U parameters, string connectionString)
    {
        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            IEnumerable<T> record = connection.Query<T>(sqlStatement, parameters);

            return record;
        }
    }

    public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
    {
        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Execute(sqlStatement, parameters);
        }
    }
}
