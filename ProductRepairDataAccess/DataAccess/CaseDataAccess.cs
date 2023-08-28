using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.Models.Entities;
using ProductRepairDataAccess.Models.Enums;
using System.Data;

namespace ProductRepairDataAccess.DataAccess;

public class CaseDataAccess : ICaseDataAccess
{
    private readonly IDataAccessOperations _dataAccessOperations;
    private readonly IItemDataAccess _itemDataAccess;
    private readonly IConfigurationSettings _configurationSettings;
    private readonly string _connectionString;

    public CaseDataAccess(IDataAccessOperations dataAccess, IItemDataAccess itemDataAccess, IConfigurationSettings configurationSettings)
    {
        _dataAccessOperations = dataAccess;
        _itemDataAccess = itemDataAccess;
        _configurationSettings = configurationSettings;
        _connectionString = configurationSettings.GetConnectionString();
    }

    public async Task<int> CreateCaseAsync(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus)
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

        int generatedCaseId = await _dataAccessOperations.SaveAndReturnRecordAsync<int,dynamic>(createCaseSql, createCaseParm);

        return generatedCaseId; // Return the generated CaseId
    }

    public async Task UpdateCaseStatusAsync(int caseId, string status)
    {
        Case caseModel = await GetCaseModelAsync(caseId);

        string updateCaseStatusSql = @"UPDATE [dbo].[Case]
                                        SET Status = @Status 
                                        WHERE CaseId = @CaseId";

        var updateCaseStatusParm = new
            {
                CaseId = caseId,
                Status = status
            };

       await _dataAccessOperations.SaveDataAsync<dynamic>(updateCaseStatusSql, updateCaseStatusParm);
    }

    public async Task<Case> GetCaseModelAsync(int caseId)
    {
        Case caseModel = new Case();

        caseModel.CaseId = caseId;

        string caseModelSql = @"SELECT * FROM [dbo].[Case]
                                WHERE CaseId = @CaseId";

        var caseModelParameters = new { CaseId = caseId };

        caseModel = await BuildCaseModelAsync(await _dataAccessOperations.SaveAndReturnRecordAsync<Case, dynamic>(caseModelSql, caseModelParameters));

        return caseModel;
    }

    public async Task<Case> BuildCaseModelAsync(Case caseModel)
    {
        var caseItems = await _itemDataAccess.GetItemsFromCaseAsync(caseModel.CaseId);

        if (caseItems != null && caseItems.Count > 0)
        {
            caseModel.Items.AddRange(caseItems);

            foreach (var item in caseModel.Items)
            {
                var itemIssues = await _itemDataAccess.GetItemIssueFromItemAsync(item.ItemId);

                if (itemIssues != null && itemIssues.Count > 0)
                {
                    item.ItemIssues.AddRange(itemIssues);
                }
            }
        }
        return caseModel;
    }

    public async Task<List<Case>> GetCasesAsync(string caseStatus, string accountId)
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

        caseModels = await _dataAccessOperations.LoadRecordsAsync<Case, dynamic>(getCasesSql, getCasesParm);
       

        foreach (var caseModel in caseModels)
        {
            BuildCaseModelAsync(caseModel);
        }

        return caseModels;
    }

    public async Task AddCustomerInformationToCaseAsync(Case caseModel)
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

        await _dataAccessOperations.SaveDataAsync<dynamic>(addCustomerInformationSql, addCustomerInformationParm);
    }

    public async Task RemoveCustomerInformationFromCaseAsync(int caseId)
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

         await _dataAccessOperations.SaveDataAsync<dynamic>(removeCustomerInformationSql, removeCustomerInformationParm);
        
    }
}
