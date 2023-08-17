using ProductRepairDataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models
{
    public class CaseModel
    {
        public string CaseId { get; set; }
        public AccountModel Account { get; set; }
        public IncidentType IncidentType { get; set; }
        public SalesChannel SalesChannel { get; set; }
        public CaseStatus Status { get; set; }
        public List<ItemModel> Items { get; set; }
        public CustomerModel Customer { get; set; }
        public List<CommentModel>? Comments { get; set; }
    }
}
