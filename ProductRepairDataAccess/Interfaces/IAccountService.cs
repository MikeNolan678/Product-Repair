using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Interfaces
{
    public interface IAccountService
    {
        string GetUserAccountId(string dbConnection);
    }
}
