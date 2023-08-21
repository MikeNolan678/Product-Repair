using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductRepairDataAccess.Models.Enums;
using ProductRepairDataAccess.SQL;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Common.CommandTrees;
using Dapper;

namespace ProductRepairDataAccess.Helpers
{
    public class ItemHelpers
    {
        public static void AddItemToCase(NewCaseModel newCaseModel, string dbConnection)
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
                DataAccess.SaveData<dynamic>(addItemToCaseSql, addItemToCaseParm, dbConnection);
            }
        }

        public static List<ItemModel> GetItemsFromCase (int caseId, string dbConnection)
        {
            var itemList = new List<ItemModel>();

            string itemsFromCaseSql = @"SELECT * FROM [dbo].[Items] WHERE CaseId = @CaseId";

            var itemsFromCaseParm = new
            {
                CaseId = caseId
            };

           itemList = DataAccess.LoadRecord<ItemModel, dynamic>
                        (itemsFromCaseSql, 
                        itemsFromCaseParm,
                        dbConnection).ToList();

            foreach (var item in itemList)
            {
                if (item.ItemIssues != null && item.ItemIssues.Count > 0)
                {
                    List<ItemIssueModel> itemIssues = new List<ItemIssueModel>();

                    itemIssues = GetItemIssueFromItem(item.ItemId, dbConnection);

                    item.ItemIssues.AddRange(itemIssues);
                }
            }
            return itemList;
        }

        public static List<ItemIssueModel> GetItemIssueFromItem (Guid ItemId, string dbConnection)
        {
            List<ItemIssueModel> itemIssues = new List<ItemIssueModel>();

            string itemIssueFromItemSql = @"SELECT * FROM [dbo].[ItemIssues]
                                       WHERE ItemId = @ItemId";

            var itemIssueFromItemParm = new
            {
                ItemId = ItemId
            };

            itemIssues = DataAccess.LoadRecord<ItemIssueModel, dynamic>
                        (itemIssueFromItemSql,
                        itemIssueFromItemParm,
                        dbConnection).ToList();

            return itemIssues;

        }

        public static ItemModel GetItemModel(Guid itemId, string dbConnection)
        {
            ItemModel itemModel = new ItemModel();

            string itemModelSql = @"SELECT * FROM [dbo].[Items]
                                       WHERE ItemId = @ItemId";

            var itemModelParameters = new { ItemId = itemId };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                itemModel = connection.QuerySingleOrDefault<ItemModel>(itemModelSql, itemModelParameters);
            }

            return itemModel;
        }

        public static void AddItemIssueToItem(NewItemIssueModel newItemIssue, string dbConnection)
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
                DataAccess.SaveData<dynamic>(addItemToCaseSql, addItemToCaseParm, dbConnection);
            }
        }
    }
}
