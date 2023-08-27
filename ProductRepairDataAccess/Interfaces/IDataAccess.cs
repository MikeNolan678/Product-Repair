namespace ProductRepairDataAccess.Interfaces;

public interface IDataAccess
{
    public IEnumerable<T> LoadRecord<T, U>(string sqlStatement, U parameters, string connectionString);
    public void SaveData<T>(string sqlStatement, T parameters, string connectionString);
}