using ProductRepairDataAccess.Models.Entities;
using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Interfaces
{
    public interface ICaseDataAccess
    {
        void AddCustomerInformationToCase(Case caseModel, string dbConnection);
        Case BuildCaseModel(string dbConnection, Case caseModel);
        int CreateCase(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus, string dbConnection);
        Case GetCaseModel(int caseId, string dbConnection);
        List<Case> GetCases(string caseStatus, string accountId, string dbConnection);
        void RemoveCustomerInformationFromCase(int caseId, string dbConnection);
        void UpdateCaseStatus(int caseId, string status, string dbConnection);
    }
}