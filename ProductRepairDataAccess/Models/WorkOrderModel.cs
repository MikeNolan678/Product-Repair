using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models
{
    public class WorkOrderModel
    {
        public string WorkOrderId { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}
