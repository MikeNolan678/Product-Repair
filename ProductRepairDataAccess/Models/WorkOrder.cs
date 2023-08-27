using ProductRepairDataAccess.Models.Entities;

namespace ProductRepairDataAccess.Models;

public class WorkOrder
{
    public string WorkOrderId { get; set; }
    public List<Comment> Comments { get; set; }
}
