using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Operations;
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

            int caseId = CaseDataAccess.CreateCase(
                        accountId,
                        IncidentType.RepairRequest,
                        SalesChannel.Dealer,
                        CaseStatus.Draft,
                        _dbConnection);

            Case caseModel = new Case
            {
                CaseId = caseId,
                AccountId = accountId
            };

            return View("ViewCase", caseModel);
        }

        // GET: CaseController
        public ActionResult ViewCase(int caseId)
        {
            Case caseModel = CaseDataAccess.GetCaseModel(caseId, _dbConnection);

            return View(caseModel);
        }

        public ActionResult NewItem(int caseId)
        {
            Case caseModel = CaseDataAccess.GetCaseModel(caseId, _dbConnection);

            return View(caseModel);
        }

        [HttpPost]
        public ActionResult AddItem(NewCase newItem)
        {
            // Process the other fields submitted in the form and update the datasources
            ItemDataAccess.AddItemToCase(newItem, _dbConnection);

            Case caseModel = CaseDataAccess.GetCaseModel(newItem.CaseId, _dbConnection);

            return View("ViewCase", caseModel);
        }

        public ActionResult NewItemIssue(int caseId, Guid ItemId)
        {
            Item itemModel = ItemDataAccess.GetItemModel(ItemId, _dbConnection);

            return View(itemModel);
        }

        [HttpPost]
        public ActionResult AddItemIssue(NewItemIssue newItemIssue)
        {
            // Process the other fields submitted in the form and update the datasources
            ItemDataAccess.AddItemIssueToItem(newItemIssue, _dbConnection);

            Case caseModel = CaseDataAccess.GetCaseModel(newItemIssue.CaseId, _dbConnection);

            return View("ViewCase", caseModel);
        }

        [HttpPost]
        public ActionResult AddCustomerInformation (Case newCustomerCaseModel)
        {  
            if (Request.Form["ReceiveNotification"] == "on")
            {
                newCustomerCaseModel.ReceiveNotification = true;
            }
            else
            {
                newCustomerCaseModel.ReceiveNotification = false;
            }

            CaseDataAccess.AddCustomerInformationToCase(newCustomerCaseModel, _dbConnection);

            Case caseModel = CaseDataAccess.GetCaseModel(newCustomerCaseModel.CaseId, _dbConnection);

            return View("ViewCase", caseModel);
        }

        public ActionResult RemoveCustomerInformation (int caseId)
        {

            CaseDataAccess.RemoveCustomerInformationFromCase(caseId, _dbConnection);

            Case caseModel = CaseDataAccess.GetCaseModel(caseId, _dbConnection);

            return View("ViewCase", caseModel);
        }

        public ActionResult SubmitCase(int caseId)
        {
            CaseDataAccess.UpdateCaseStatus(caseId,"Open", _dbConnection);

            return View();
        }

        public ActionResult SaveDraft(int caseId)
        {
            Case caseModel = CaseDataAccess.GetCaseModel(caseId, _dbConnection);

            return View(caseModel);
        }

        public ActionResult DraftCases()
        {
            string accountId = _accountService.GetUserAccountId(_dbConnection);

            Cases casesModel = new Cases();

            casesModel.CasesList.AddRange(CaseDataAccess.GetCases("draft", accountId, _dbConnection));

            return View(casesModel);
        }

        public ActionResult CreateShipment(int caseId)
        {
            Case caseModel = CaseDataAccess.GetCaseModel(caseId, _dbConnection);

            return View(caseModel);
        }

        public ActionResult CancelCase(int caseId)
        {
            CaseDataAccess.UpdateCaseStatus(caseId, "Canceled", _dbConnection);

            Case caseModel = CaseDataAccess.GetCaseModel(caseId, _dbConnection);

            return View();
        }
    }
}
