using ProductRepairDataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models
{
    public class ItemModel
    {
        public string ItemNumber { get; set; }
        public string ColorCode { get; set; }
        public string Size { get; set; }
        public List<ItemIssueModel> ItemIssues { get; set; }
        public ItemStatus Status { get; set; }
        public string ItemId { get; set; }
    }
}
