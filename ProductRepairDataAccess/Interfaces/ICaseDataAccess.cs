using ProductRepairDataAccess.Models.Entities;
using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Interfaces
{
    public interface ICaseDataAccess
    {
        void AddCustomerInformationToCase(Case caseModel);
        Case BuildCaseModel(Case caseModel);
        int CreateCase(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus);
        Case GetCaseModel(int caseId);
        List<Case> GetCases(string caseStatus, string accountId);
        void RemoveCustomerInformationFromCase(int caseId);
        void UpdateCaseStatus(int caseId, string status);
    }
}