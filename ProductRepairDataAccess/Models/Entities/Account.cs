namespace ProductRepairDataAccess.Models.Entities;

public class Account
{
    public string AccountId { get; set; }
    public string AccountName { get; set; }
    public Contact Contact { get; set; }
}
