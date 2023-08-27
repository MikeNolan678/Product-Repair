using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Models;

public class Shipment
{
    public string ShipmentId { get; set; }
    public List<WorkOrder> WorkOrders { get; set; }
    public RepairCentres RepairCentre { get; set; }
}
