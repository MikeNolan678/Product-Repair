namespace ProductRepairDataAccess.Interfaces;

public interface IDataAccessOperations
{
    public IEnumerable<T> LoadRecords<T, U>(string sqlStatement, U parameters);
    public T SaveAndReturnRecord<T, U>(string sqlStatement, U parameters);
    public void SaveData<T>(string sqlStatement, T parameters);
}