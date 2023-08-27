using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Models;
using System.Data;
using ProductRepairDataAccess.Models.Enums;
using Dapper;
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.Models.Entities;

namespace ProductRepairDataAccess.DataAccess;

public class ItemDataAccess : IItemDataAccess
{
    private readonly IDataAccess _dataAccess;
    public ItemDataAccess(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public void AddItemToCase(NewCase newCaseModel, string dbConnection)
    {
        Guid id = Guid.NewGuid();

        string addItemToCaseSql = @"INSERT INTO [dbo].[Items] (ItemId, ItemNumber, ColorCode, Size, Status, CaseId) 
                                        VALUES (@ItemId, @ItemNumber, @ColorCode, @Size, 'New', @CaseId)";

        var addItemToCaseParm = new
        {
            CaseId = newCaseModel.CaseId,
            ItemNumber = newCaseModel.ItemNumber,
            ColorCode = newCaseModel.ColorCode,
            Size = newCaseModel.Size,
            Status = ItemStatus.New,
            ItemId = id,
        };

        using (IDbConnection connection = new SqlConnection(dbConnection))
        {
            _dataAccess.SaveData<dynamic>(addItemToCaseSql, addItemToCaseParm, dbConnection);
        }
    }

    public List<Item> GetItemsFromCase(int caseId, string dbConnection)
    {
        var itemList = new List<Item>();

        string itemsFromCaseSql = @"SELECT * FROM [dbo].[Items] WHERE CaseId = @CaseId";

        var itemsFromCaseParm = new
        {
            CaseId = caseId
        };

        itemList = _dataAccess.LoadRecord<Item, dynamic>
                     (itemsFromCaseSql,
                     itemsFromCaseParm,
                     dbConnection).ToList();

        foreach (var item in itemList)
        {
            if (item.ItemIssues != null && item.ItemIssues.Count > 0)
            {
                List<ItemIssue> itemIssues = new List<ItemIssue>();

                itemIssues = GetItemIssueFromItem(item.ItemId, dbConnection);

                item.ItemIssues.AddRange(itemIssues);
            }
        }
        return itemList;
    }

    public List<ItemIssue> GetItemIssueFromItem(Guid ItemId, string dbConnection)
    {
        List<ItemIssue> itemIssues = new List<ItemIssue>();

        string itemIssueFromItemSql = @"SELECT * FROM [dbo].[ItemIssues]
                                       WHERE ItemId = @ItemId";

        var itemIssueFromItemParm = new
        {
            ItemId = ItemId
        };

        itemIssues = _dataAccess.LoadRecord<ItemIssue, dynamic>
                    (itemIssueFromItemSql,
                    itemIssueFromItemParm,
                    dbConnection).ToList();

        return itemIssues;

    }

    public Item GetItemModel(Guid itemId, string dbConnection)
    {
        Item itemModel = new Item();

        string itemModelSql = @"SELECT * FROM [dbo].[Items]
                                       WHERE ItemId = @ItemId";

        var itemModelParameters = new { ItemId = itemId };

        using (IDbConnection connection = new SqlConnection(dbConnection))
        {
            itemModel = connection.QuerySingleOrDefault<Item>(itemModelSql, itemModelParameters);
        }

        return itemModel;
    }

    public void AddItemIssueToItem(NewItemIssue newItemIssue, string dbConnection)
    {
        Guid issueId = Guid.NewGuid();

        string addItemToCaseSql = @"INSERT INTO [dbo].[ItemIssues] (IssueId, ItemId, IssueCategory, IssueArea, ItemOrientation, IssueDetails ) 
                                        VALUES (@IssueId, @ItemId, @IssueCategory, @IssueArea, @ItemOrientation, @IssueDetails)";

        var addItemToCaseParm = new
        {
            IssueId = issueId,
            ItemId = newItemIssue.ItemId,
            IssueCategory = newItemIssue.IssueCategory,
            IssueArea = newItemIssue.IssueArea,
            ItemOrientation = newItemIssue.ItemOrientation,
            IssueDetails = newItemIssue.IssueDetails
        };

        using (IDbConnection connection = new SqlConnection(dbConnection))
        {
            _dataAccess.SaveData<dynamic>(addItemToCaseSql, addItemToCaseParm, dbConnection);
        }
    }
}
