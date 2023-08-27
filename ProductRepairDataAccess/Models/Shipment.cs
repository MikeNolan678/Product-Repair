using ProductRepairDataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models;

public class Shipment
{
    public string ShipmentId { get; set; }
    public List<WorkOrder> WorkOrders { get; set; }
    public RepairCentres RepairCentre { get; set; }
}
