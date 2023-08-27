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
    private readonly IConfigurationSettings _configurationSettings;
    private readonly string _connectionString;

    public CaseDataAccess(IDataAccess dataAccess, IItemDataAccess itemDataAccess, IConfigurationSettings configurationSettings)
    {
        _dataAccess = dataAccess;
        _itemDataAccess = itemDataAccess;
        _configurationSettings = configurationSettings;
        _connectionString = configurationSettings.GetConnectionString();
    }

    public int CreateCase(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus)
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

        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            int generatedCaseId = connection.QuerySingleOrDefault<int>(createCaseSql, createCaseParm);

            return generatedCaseId; // Return the generated CaseId
        }
    }

    public void UpdateCaseStatus(int caseId, string status)
    {
        Case caseModel = GetCaseModel(caseId);

        string updateCaseStatusSql = @"UPDATE [dbo].[Case]
                                        SET Status = @Status 
                                        WHERE CaseId = @CaseId";

        var updateCaseStatusParm = new
            {
                CaseId = caseId,
                Status = status
            };

       _dataAccess.SaveData<dynamic>(updateCaseStatusSql, updateCaseStatusParm);
    }

    public Case GetCaseModel(int caseId)
    {
        Case caseModel = new Case();

        caseModel.CaseId = caseId;

        string caseModelSql = @"SELECT * FROM [dbo].[Case]
                                       WHERE CaseId = @CaseId";

        var caseModelParameters = new { CaseId = caseId };

        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            caseModel = connection.QuerySingleOrDefault<Case>(caseModelSql, caseModelParameters);
        }
        return BuildCaseModel(caseModel);
    }

    public Case BuildCaseModel(Case caseModel)
    {
        var caseItems = _itemDataAccess.GetItemsFromCase(caseModel.CaseId);

        if (caseItems != null && caseItems.Count > 0)
        {
            caseModel.Items.AddRange(caseItems);

            foreach (var item in caseModel.Items)
            {
                var itemIssues = _itemDataAccess.GetItemIssueFromItem(item.ItemId);

                if (itemIssues != null && itemIssues.Count > 0)
                {
                    item.ItemIssues.AddRange(itemIssues);
                }
            }
        }
        return caseModel;
    }

    public List<Case> GetCases(string caseStatus, string accountId)
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

        caseModels = _dataAccess.LoadRecord<Case, dynamic>(getCasesSql, getCasesParm).ToList();
       

        foreach (var caseModel in caseModels)
        {
            BuildCaseModel(caseModel);
        }

        return caseModels;
    }

    public void AddCustomerInformationToCase(Case caseModel)
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

        _dataAccess.SaveData<dynamic>(addCustomerInformationSql, addCustomerInformationParm);
    }

    public void RemoveCustomerInformationFromCase(int caseId)
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

         _dataAccess.SaveData<dynamic>(removeCustomerInformationSql, removeCustomerInformationParm);
        
    }
}
