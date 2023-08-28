using ProductRepairDataAccess.Models.Entities;
using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDataAccess.Interfaces
{
    public interface ICaseDataAccess
    {
        Task AddCustomerInformationToCaseAsync(Case caseModel);
        Task<Case> BuildCaseModelAsync(Case caseModel);
        Task<int> CreateCaseAsync(string accountId, IncidentType incidentType, SalesChannel salesChannel, CaseStatus caseStatus);
        Task<Case> GetCaseModelAsync(int caseId);
        Task<List<Case>> GetCasesAsync(string caseStatus, string accountId);
        Task RemoveCustomerInformationFromCaseAsync(int caseId);
        Task UpdateCaseStatusAsync(int caseId, string status);
    }
}