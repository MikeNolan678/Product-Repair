namespace ProductRepairDataAccess.Interfaces;

public interface IDataAccessOperations
{
    public Task<List<T>> LoadRecordsAsync<T, U>(string sqlStatement, U parameters);
    public Task<T> SaveAndReturnRecordAsync<T, U>(string sqlStatement, U parameters);
    public Task SaveDataAsync<T>(string sqlStatement, T parameters);
}