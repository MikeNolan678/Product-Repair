using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.Models.Entities;
using ProductRepairDataAccess.Models.Enums;
using System.Data;

namespace ProductRepairDataAccess.DataAccess;

public class CaseDataAccess : ICaseDataAccess
{
    private readonly IDataAccess _dataAccess;
    private readonly IItemDataAccess _itemDataAccess;

    public CaseDataAccess(IDataAccess dataAccess, IItemDataAccess itemDataAccess)
    {
        _dataAccess = dataAccess;
        _itemDataAccess = itemDataAccess;
    }


    public int CreateCase(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus, string dbConnection)
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

    public void UpdateCaseStatus(int caseId, string status, string dbConnection)
    {
        Case caseModel = GetCaseModel(caseId, dbConnection);

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
            _dataAccess.SaveData<dynamic>(updateCaseStatusSql, updateCaseStatusParm, dbConnection);
        }
    }

    public Case GetCaseModel(int caseId, string dbConnection)
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

    public Case BuildCaseModel(string dbConnection, Case caseModel)
    {
        var caseItems = _itemDataAccess.GetItemsFromCase(caseModel.CaseId, dbConnection);

        if (caseItems != null && caseItems.Count > 0)
        {
            caseModel.Items.AddRange(caseItems);

            foreach (var item in caseModel.Items)
            {
                var itemIssues = _itemDataAccess.GetItemIssueFromItem(item.ItemId, dbConnection);

                if (itemIssues != null && itemIssues.Count > 0)
                {
                    item.ItemIssues.AddRange(itemIssues);
                }
            }
        }
        return caseModel;
    }

    public List<Case> GetCases(string caseStatus, string accountId, string dbConnection)
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
            caseModels = _dataAccess.LoadRecord<Case, dynamic>(getCasesSql, getCasesParm, dbConnection).ToList();
        }

        foreach (var caseModel in caseModels)
        {
            BuildCaseModel(dbConnection, caseModel);
        }

        return caseModels;
    }

    public void AddCustomerInformationToCase(Case caseModel, string dbConnection)
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
            _dataAccess.SaveData<dynamic>(addCustomerInformationSql, addCustomerInformationParm, dbConnection);
        }
    }

    public void RemoveCustomerInformationFromCase(int caseId, string dbConnection)
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
            _dataAccess.SaveData<dynamic>(removeCustomerInformationSql, removeCustomerInformationParm, dbConnection);
        }
    }
}
