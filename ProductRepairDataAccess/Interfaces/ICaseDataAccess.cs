using ProductRepairDataAccess.Models.Entities;
using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Interfaces
{
    public interface ICaseDataAccess
    {
        Task AddCustomerInformationToCase(Case caseModel);
        Task<Case> BuildCaseModel(Case caseModel);
        Task<int> CreateCaseAsync(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus);
        Task<Case> GetCaseModel(int caseId);
        Task<List<Case>> GetCases(string caseStatus, string accountId);
        Task RemoveCustomerInformationFromCase(int caseId);
        Task UpdateCaseStatusAsync(int caseId, string status);
    }
}