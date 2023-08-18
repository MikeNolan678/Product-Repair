using Microsoft.AspNetCore.Http;
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

            ViewData["CaseId"] = caseId;
            ViewData["AccountId"] = accountId;

            return View();
        }

        // GET: CaseController
        public ActionResult ViewAllCases()
        {
            return View();
        }

        // GET: CaseController
        public ActionResult ViewCase()
        {
            return View();
        }

        public ActionResult AddItem(ItemModel item)
        {
            // Process the item data and add it to your data source

            // Return the rendered item HTML as a partial view
            return PartialView("_AddedItemPartial", item);
        }
    }
}
