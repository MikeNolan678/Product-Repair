using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Enums;
using ProductRepairDataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProductRepairDataAccess.Helpers
{
    public class CaseDataAccess
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
            Case caseModel = CaseDataAccess.GetCaseModel(caseId, dbConnection);

            string updateCaseStatusSql = @"UPDATE [dbo].[Case]
                                        SET Status = @Status 
                                        WHERE CaseId = @CaseId";

            var updateCaseStatusParm = new 
            {
                CaseId = caseId,
                Status = status
            };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                DataAccess.SaveData<dynamic>(updateCaseStatusSql, updateCaseStatusParm, dbConnection);
            }
        }

        public static Case GetCaseModel(int caseId, string dbConnection)
        {
            Case caseModel = new Case();

            caseModel.CaseId = caseId;

            string caseModelSql = @"SELECT * FROM [dbo].[Case]
                                       WHERE CaseId = @CaseId";

            var caseModelParameters = new { CaseId = caseId };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                caseModel = connection.QuerySingleOrDefault<Case>(caseModelSql, caseModelParameters);
            }
            return BuildCaseModel(dbConnection, caseModel);
        }

        public static Case BuildCaseModel (string dbConnection, Case caseModel)
        {
            var caseItems = ItemDataAccess.GetItemsFromCase(caseModel.CaseId, dbConnection);

            if (caseItems != null && caseItems.Count > 0)
            {
                caseModel.Items.AddRange(caseItems);

                foreach (var item in caseModel.Items)
                {
                    var itemIssues = ItemDataAccess.GetItemIssueFromItem(item.ItemId, dbConnection);

                    if (itemIssues != null && itemIssues.Count > 0)
                    {
                        item.ItemIssues.AddRange(itemIssues);
                    }
                }
            }
            return caseModel;
        }

        public static List<Case> GetCases(string caseStatus, string accountId, string dbConnection)
        {
            List<Case> caseModels = new List<Case>();

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
                caseModels = DataAccess.LoadRecord<Case, dynamic>(getCasesSql, getCasesParm, dbConnection).ToList();
            }

            foreach (var caseModel in caseModels) 
            {
                BuildCaseModel(dbConnection,caseModel);
            }

            return caseModels;
        }

        public static void AddCustomerInformationToCase(Case caseModel, string dbConnection)
        {
            string addCustomerInformationSql = @"UPDATE [dbo].[Case]
                                        SET CustomerFirstName = @CustomerFirstName, 
                                            CustomerLastName = @CustomerLastName, 
                                            CustomerEmailAddress = @CustomerEmailAddress,
                                            ReceiveNotification = @ReceiveNotification
                                        WHERE CaseId = @CaseId";

            var addCustomerInformationParm = new
            {
                CaseId = caseModel.CaseId,
                CustomerFirstName = caseModel.CustomerFirstName,
                CustomerLastName = caseModel.CustomerLastName,
                CustomerEmailAddress = caseModel.CustomerEmailAddress,
                ReceiveNotification = caseModel.ReceiveNotification
            };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                DataAccess.SaveData<dynamic>(addCustomerInformationSql, addCustomerInformationParm, dbConnection);
            }
        }

        public static void RemoveCustomerInformationFromCase(int caseId, string dbConnection)
        {
            string removeCustomerInformationSql = @"UPDATE [dbo].[Case]
                                        SET CustomerFirstName = @CustomerFirstName, 
                                            CustomerLastName = @CustomerLastName, 
                                            CustomerEmailAddress = @CustomerEmailAddress,
                                            ReceiveNotification = @ReceiveNotification
                                        WHERE CaseId = @CaseId";

            var removeCustomerInformationParm = new
            {
                CaseId = caseId,
                CustomerFirstName = (string?)null,
                CustomerLastName = (string?)null,
                CustomerEmailAddress = (string?)null,
                ReceiveNotification = (bool?)null
            };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                DataAccess.SaveData<dynamic>(removeCustomerInformationSql, removeCustomerInformationParm, dbConnection);
            }
        }
    }
}
