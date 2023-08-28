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
    private readonly IDataAccessOperations _dataAccessOperations;
    private readonly IConfigurationSettings _configurationSettings;
    private readonly string _connectionString;
    public ItemDataAccess(IDataAccessOperations dataAccess, IConfigurationSettings configurationSettings)
    {
        _dataAccessOperations = dataAccess;
        _configurationSettings = configurationSettings;
        _connectionString = configurationSettings.GetConnectionString();

    }

    public void AddItemToCase(NewCase newCaseModel)
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
        
        _dataAccessOperations.SaveData<dynamic>(addItemToCaseSql, addItemToCaseParm);
    }

    public List<Item> GetItemsFromCase(int caseId)
    {
        var itemList = new List<Item>();

        string itemsFromCaseSql = @"SELECT * FROM [dbo].[Items] WHERE CaseId = @CaseId";

        var itemsFromCaseParm = new
        {
            CaseId = caseId
        };

        itemList = _dataAccessOperations.LoadRecords<Item, dynamic>
                     (itemsFromCaseSql,
                     itemsFromCaseParm).ToList();

        foreach (var item in itemList)
        {
            if (item.ItemIssues != null && item.ItemIssues.Count > 0)
            {
                List<ItemIssue> itemIssues = new List<ItemIssue>();

                itemIssues = GetItemIssueFromItem(item.ItemId);

                item.ItemIssues.AddRange(itemIssues);
            }
        }
        return itemList;
    }

    public List<ItemIssue> GetItemIssueFromItem(Guid ItemId)
    {
        List<ItemIssue> itemIssues = new List<ItemIssue>();

        string itemIssueFromItemSql = @"SELECT * FROM [dbo].[ItemIssues]
                                       WHERE ItemId = @ItemId";

        var itemIssueFromItemParm = new
        {
            ItemId = ItemId
        };

        itemIssues = _dataAccessOperations.LoadRecords<ItemIssue, dynamic>
                    (itemIssueFromItemSql,
                    itemIssueFromItemParm).ToList();

        return itemIssues;

    }

    public Item GetItemModel(Guid itemId)
    {
        Item itemModel = new Item();

        string itemModelSql = @"SELECT * FROM [dbo].[Items]
                                       WHERE ItemId = @ItemId";

        var itemModelParameters = new { ItemId = itemId };

        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            itemModel = connection.QuerySingleOrDefault<Item>(itemModelSql, itemModelParameters);
        }

        return itemModel;
    }

    public void AddItemIssueToItem(NewItemIssue newItemIssue)
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

        _dataAccessOperations.SaveData<dynamic>(addItemToCaseSql, addItemToCaseParm);
    }
}
