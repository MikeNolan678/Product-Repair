using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Enums;
using ProductRepairDataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Helpers
{
    public class CaseHelpers
    {
        public static int CreateCase(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus, string dbConnection)
        {
            string createCaseSql = @"INSERT INTO [dbo].[Case] (AccountId, IncidentType, SalesChannel, Status)
                             VALUES
                             (@Id, @IncidentType, @SalesChannel, 'Draft');
                             SELECT SCOPE_IDENTITY();"; // Retrieve the generated CaseId

            var createCaseParm = new
            {
                Id = accountId,
                IncidentType = incidentType,
                SalesChannel = salesChannel,
                CaseStatus = caseStatus
            };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                int generatedCaseId = connection.QuerySingleOrDefault<int>(createCaseSql, createCaseParm);

                return generatedCaseId; // Return the generated CaseId
            }
        }

        public static void UpdateCaseStatus(int caseId,string status, string dbConnection)
        {
            CaseModel caseModel = CaseHelpers.GetCaseModel(caseId, dbConnection);

            string submitCaseSql = @"UPDATE [dbo].[Case]
                                        SET Status = @Status 
                                        WHERE CaseId = @CaseId";

            var submitCaseParm = new 
            {
                CaseId = caseId,
                Status = status
            };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                DataAccess.SaveData<dynamic>(submitCaseSql, submitCaseParm,dbConnection);
            }
        }

        public static CaseModel GetCaseModel(int caseId, string dbConnection)
        {
            CaseModel caseModel = new CaseModel();

            caseModel.CaseId = caseId;

            string caseModelSql = @"SELECT * FROM [dbo].[Case]
                                       WHERE CaseId = @CaseId";

            var caseModelParameters = new { CaseId = caseId };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                caseModel = connection.QuerySingleOrDefault<CaseModel>(caseModelSql, caseModelParameters);
            }
            return BuildCaseModel(dbConnection, caseModel);
        }

        public static CaseModel BuildCaseModel (string dbConnection, CaseModel caseModel)
        {
            var caseItems = ItemHelpers.GetItemsFromCase(caseModel.CaseId, dbConnection);

            if (caseItems != null && caseItems.Count > 0)
            {
                caseModel.Items.AddRange(caseItems);

                foreach (var item in caseModel.Items)
                {
                    var itemIssues = ItemHelpers.GetItemIssueFromItem(item.ItemId, dbConnection);

                    if (itemIssues != null && itemIssues.Count > 0)
                    {
                        item.ItemIssues.AddRange(itemIssues);
                    }
                }
            }
            return caseModel;
        }

        public static List<CaseModel> GetCases(string caseStatus, string accountId, string dbConnection)
        {
            List<CaseModel> caseModels = new List<CaseModel>();

            string getCasesSql = @"SELECT * FROM [dbo].[Case]
                                    WHERE AccountId = @AccountId
                                    AND Status = @CaseStatus";

            var getCasesParm = new
            { 
                AccountId = accountId, 
                CaseStatus = caseStatus
            };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                caseModels = DataAccess.LoadRecord<CaseModel, dynamic>(getCasesSql, getCasesParm, dbConnection).ToList();
            }

            foreach (var caseModel in caseModels) 
            {
                BuildCaseModel(dbConnection,caseModel);
            }

            return caseModels;
        }
    }
}
