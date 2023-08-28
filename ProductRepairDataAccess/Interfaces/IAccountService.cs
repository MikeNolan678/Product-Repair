namespace ProductRepairDataAccess.Interfaces;

public interface IAccountService
{
    Task<string> GetUserAccountId();
}
