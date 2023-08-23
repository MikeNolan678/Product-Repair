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
        public int CaseId { get; set; }
        public string AccountId { get; set; }
        public IncidentType IncidentType { get; set; }
        public SalesChannel SalesChannel { get; set; }
        public CaseStatus Status { get; set; }
        public List<ItemModel> Items { get; set; } = new List<ItemModel>();
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerEmailAddress { get; set; }
        public Language Language { get; set; }
        public bool? ReceiveNotification { get; set; }
        public List<CommentModel>? Comments { get; set; } = new List<CommentModel>();
    }
}
