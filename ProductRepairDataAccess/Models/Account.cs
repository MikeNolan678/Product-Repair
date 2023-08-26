using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models
{
    public class Account
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public Contact Contact { get; set; }
    }
}
