using Microsoft.AspNetCore.Http;
using ProductRepairDataAccess.Interfaces;
using System.Security.Claims;

namespace ProductRepairDataAccess.Services;

public class AccountService : IAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataAccess _dataAccess;

    public AccountService(IHttpContextAccessor httpContextAccessor, IDataAccess dataAccess)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataAccess = dataAccess;
    }

    public string GetUserAccountId(string dbConnection)
    {
        string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        string getUserAccountIdSql = @"SELECT AccountId
                            FROM AspNetUsers
                            WHERE Id = @Id;";

        var getUserAccountParm = new
        {
            Id = userId
        };

        var accountId = _dataAccess.LoadRecord<string, dynamic>(getUserAccountIdSql, getUserAccountParm, dbConnection);

        return accountId.FirstOrDefault(); // Return the AccountId
    }
}
