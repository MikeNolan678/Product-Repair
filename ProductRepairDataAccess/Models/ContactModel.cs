using ProductRepairDataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models
{
    public class ContactModel
    {
        public int ContactId { get; set; }
        public ContactType Type { get; set; }
        public string ContactName { get; set; }
        public string ContactEmailAddress { get; set; }
    }
}
