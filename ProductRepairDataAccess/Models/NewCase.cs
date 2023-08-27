using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models;

public class NewCase
{
    public string ItemNumber { get; set; }
    public string ColorCode { get; set; }
    public string Size { get; set; }
    public int CaseId { get; set; }
    public string AccountId { get; set; }
}
