﻿using Dapper;
using Microsoft.Data.SqlClient;
using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Enums;
using ProductRepairDataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public static void SubmitCase(CaseModel caseModel, string dbConnection)
        {

        }

        public static CaseModel GetCaseModel(int caseId, string dbConnection)
        {
            CaseModel caseModel = new CaseModel();

            string caseModelSql = @"SELECT * FROM [dbo].[Case]
                                       WHERE CaseId = @CaseId";

            var caseModelParameters = new { CaseId = caseId };

            using (IDbConnection connection = new SqlConnection(dbConnection))
            {
                caseModel = connection.QuerySingleOrDefault<CaseModel>(caseModelSql, caseModelParameters);
            }

            var caseItems = ItemHelpers.GetItemsFromCase(caseId, dbConnection);

            if (caseItems != null && caseItems.Count > 0)
            {
                caseModel.Items.AddRange(caseItems);
            }

            return caseModel; // Return with the generated CaseId
        }
    }
}