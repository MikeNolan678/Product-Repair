﻿using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Models.Entities;

public class Item
{
    public string ItemNumber { get; set; }
    public string ColorCode { get; set; }
    public string Size { get; set; }
    public List<ItemIssue>? ItemIssues { get; set; } = new List<ItemIssue>();
    public ItemStatus Status { get; set; }
    public Guid ItemId { get; set; }
    public int CaseId { get; set; }
}
