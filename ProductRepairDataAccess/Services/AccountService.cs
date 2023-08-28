using Microsoft.AspNetCore.Http;
using ProductRepairDataAccess.Interfaces;
using System.Security.Claims;

namespace ProductRepairDataAccess.Services;

public class AccountService : IAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataAccessOperations _dataAccess;
    private readonly IConfigurationSettings _configurationSettings;
    private readonly string _connectionString;

    public AccountService(IHttpContextAccessor httpContextAccessor, IDataAccessOperations dataAccess, IConfigurationSettings configurationSettings)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataAccess = dataAccess;
        _configurationSettings = configurationSettings;
        _connectionString = configurationSettings.GetConnectionString();
    }

    public string GetUserAccountId()
    {
        string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        string getUserAccountIdSql = @"SELECT AccountId
                            FROM AspNetUsers
                            WHERE Id = @Id;";

        var getUserAccountParm = new
        {
            Id = userId
        };

        var accountId = _dataAccess.LoadRecords<string, dynamic>(getUserAccountIdSql, getUserAccountParm);

        return accountId.FirstOrDefault(); // Return the AccountId
    }
}
