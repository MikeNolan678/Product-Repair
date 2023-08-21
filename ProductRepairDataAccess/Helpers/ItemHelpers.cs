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


            return itemList;
        }
    }
}
