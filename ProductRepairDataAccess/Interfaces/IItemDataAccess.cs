using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Entities;

namespace ProductRepairDataAccess.Interfaces
{
    public interface IItemDataAccess
    {
        Task AddItemIssueToItemAsync(NewItemIssue newItemIssue);
        Task AddItemToCaseAsync(NewCase newCaseModel);
        Task<List<ItemIssue>> GetItemIssueFromItemAsync(Guid ItemId);
        Task<Item> GetItemModelAsync(Guid itemId);
        Task<List<Item>> GetItemsFromCaseAsync(int caseId);
    }
}