using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Models.Entities;

public class ItemIssue
{
    public IssueCategory IssueCategory { get; set; }
    public IssueArea IssueArea { get; set; }
    public ItemOrientation ItemOrientation { get; set; }
    public string IssueDetails { get; set; }
}
