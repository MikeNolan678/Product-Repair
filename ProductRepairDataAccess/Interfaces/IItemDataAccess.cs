using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Entities;

namespace ProductRepairDataAccess.Interfaces
{
    public interface IItemDataAccess
    {
        void AddItemIssueToItem(NewItemIssue newItemIssue, string dbConnection);
        void AddItemToCase(NewCase newCaseModel, string dbConnection);
        List<ItemIssue> GetItemIssueFromItem(Guid ItemId, string dbConnection);
        Item GetItemModel(Guid itemId, string dbConnection);
        List<Item> GetItemsFromCase(int caseId, string dbConnection);
    }
}