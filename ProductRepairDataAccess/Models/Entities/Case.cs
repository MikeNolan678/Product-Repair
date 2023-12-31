﻿using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Models.Entities;

public class Case
{
    public int CaseId { get; set; }
    public string AccountId { get; set; }
    public IncidentType IncidentType { get; set; }
    public SalesChannel SalesChannel { get; set; }
    public CaseStatus Status { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public string? CustomerFirstName { get; set; }
    public string? CustomerLastName { get; set; }
    public string? CustomerEmailAddress { get; set; }
    public Language Language { get; set; }
    public bool? ReceiveNotification { get; set; }
    public List<Comment>? Comments { get; set; } = new List<Comment>();
}
