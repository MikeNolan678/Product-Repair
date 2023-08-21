﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductRepairDataAccess;
using ProductRepairDataAccess.Helpers;
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Enums;
using ProductRepairDataAccess.Services;
using ProductRepairDataAccess.SQL;

namespace ProductRepairDealerUI.Controllers
{
    public class CaseController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly string _dbConnection;

        public CaseController(IAccountService accountService, IConfiguration configuration)
        {
            var configSettings = Configuration.GetConfigurationSettings(configuration);

            _dbConnection = configSettings.DbConnection;
            _accountService = accountService;
        }

        // GET: CaseController
        public ActionResult CreateCase()
        {
            string accountId = _accountService.GetUserAccountId(_dbConnection);

            int caseId = CaseHelpers.CreateCase(
                        accountId,
                        IncidentType.RepairRequest,
                        SalesChannel.Dealer,
                        CaseStatus.Draft,
                        _dbConnection);

            CaseModel caseModel = new CaseModel
            {
                CaseId = caseId,
                AccountId = accountId
            };

            return View("ViewCase", caseModel);
        }

        // GET: CaseController
        public ActionResult ViewCase(int caseId)
        {
            CaseModel caseModel = CaseHelpers.GetCaseModel(caseId, _dbConnection);

            return View(caseModel);
        }

        public ActionResult NewItem(int caseId)
        {
            CaseModel caseModel = CaseHelpers.GetCaseModel(caseId, _dbConnection);

            return View(caseModel);
        }

        [HttpPost]
        public ActionResult AddItem(NewCaseModel newItem)
        {
            // Process the other fields submitted in the form and update the datasources
            ItemHelpers.AddItemToCase(newItem, _dbConnection);

            CaseModel caseModel = CaseHelpers.GetCaseModel(newItem.CaseId, _dbConnection);

            return View("ViewCase", caseModel);
        }
        

        
    }
}