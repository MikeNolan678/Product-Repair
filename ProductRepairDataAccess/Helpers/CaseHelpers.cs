using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Models.Enums;
using ProductRepairDataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                             (@Id, @IncidentType, @SalesChannel, 'Open');
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
    }
}
