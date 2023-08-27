using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Entities;

namespace ProductRepairDataAccess.Interfaces
{
    public interface IItemDataAccess
    {
        void AddItemIssueToItem(NewItemIssue newItemIssue);
        void AddItemToCase(NewCase newCaseModel);
        List<ItemIssue> GetItemIssueFromItem(Guid ItemId);
        Item GetItemModel(Guid itemId);
        List<Item> GetItemsFromCase(int caseId);
    }
}