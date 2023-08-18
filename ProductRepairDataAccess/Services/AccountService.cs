using Microsoft.AspNetCore.Http;
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserAccountId(string dbConnection)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            string Sql = @"SELECT AccountId
                            FROM AspNetUsers
                            WHERE Id = @Id;";

            var Parm = new
            {
                Id = userId
            };

            var accountId = DataAccess.LoadRecord<string, dynamic>(Sql, Parm, dbConnection);

            return accountId.FirstOrDefault(); // Return the AccountId
        }
    }
}
