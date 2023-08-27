using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Models.Entities;

public class Contact
{
    public int ContactId { get; set; }
    public ContactType Type { get; set; }
    public string ContactName { get; set; }
    public string ContactEmailAddress { get; set; }
}
