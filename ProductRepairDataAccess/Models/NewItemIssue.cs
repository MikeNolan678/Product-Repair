using ProductRepairDataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Models;

public class NewItemIssue
{
    public int CaseId { get; set; }
    public Guid ItemId { get; set; }
    public IssueArea IssueArea { get; set; }
    public IssueCategory IssueCategory { get; set; }
    public ItemOrientation ItemOrientation { get; set; }
    public string IssueDetails { get; set; }
}
