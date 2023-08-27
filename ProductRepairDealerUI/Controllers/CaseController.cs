using Microsoft.AspNetCore.Mvc;
using ProductRepairDataAccess.DataAccess;
using ProductRepairDataAccess.Interfaces;
using ProductRepairDataAccess.Models;
using ProductRepairDataAccess.Models.Entities;
using ProductRepairDataAccess.Models.Enums;

namespace ProductRepairDealerUI.Controllers;

public class CaseController : Controller
{
    private readonly IAccountService _accountService;
    private readonly string _dbConnection;
    private readonly IDataAccess _dataAccess;
    private readonly ICaseDataAccess _caseDataAccess;
    private readonly IItemDataAccess _itemDataAccess;

    public CaseController(
        IAccountService accountService, 
        IConfiguration configuration, 
        IDataAccess dataAccess, 
        ICaseDataAccess caseDataAccess, 
        IItemDataAccess itemDataAccess)
    {
        var configSettings = Configuration.GetConfigurationSettings(configuration);

        _dbConnection = configSettings.DbConnection;
        _accountService = accountService;
        _dataAccess = dataAccess;
        _caseDataAccess = caseDataAccess;
        _itemDataAccess = itemDataAccess;
    }

    // GET: CaseController
    public ActionResult CreateCase()
    {
        string accountId = _accountService.GetUserAccountId(_dbConnection);

        int caseId = _caseDataAccess.CreateCase(
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
        Case caseModel = _caseDataAccess.GetCaseModel(caseId, _dbConnection);

        return View(caseModel);
    }

    public ActionResult NewItem(int caseId)
    {
        Case caseModel = _caseDataAccess.GetCaseModel(caseId, _dbConnection);

        return View(caseModel);
    }

    [HttpPost]
    public ActionResult AddItem(NewCase newItem)
    {
        // Process the other fields submitted in the form and update the datasources
        _itemDataAccess.AddItemToCase(newItem, _dbConnection);

        Case caseModel = _caseDataAccess.GetCaseModel(newItem.CaseId, _dbConnection);

        return View("ViewCase", caseModel);
    }

    public ActionResult NewItemIssue(int caseId, Guid ItemId)
    {
        Item itemModel =_itemDataAccess.GetItemModel(ItemId, _dbConnection);

        return View(itemModel);
    }

    [HttpPost]
    public ActionResult AddItemIssue(NewItemIssue newItemIssue)
    {
        // Process the other fields submitted in the form and update the datasources
        _itemDataAccess.AddItemIssueToItem(newItemIssue, _dbConnection);

        Case caseModel = _caseDataAccess.GetCaseModel(newItemIssue.CaseId, _dbConnection);

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

        _caseDataAccess.AddCustomerInformationToCase(newCustomerCaseModel, _dbConnection);

        Case caseModel = _caseDataAccess.GetCaseModel(newCustomerCaseModel.CaseId, _dbConnection);

        return View("ViewCase", caseModel);
    }

    public ActionResult RemoveCustomerInformation (int caseId)
    {

        _caseDataAccess.RemoveCustomerInformationFromCase(caseId, _dbConnection);

        Case caseModel = _caseDataAccess.GetCaseModel(caseId, _dbConnection);

        return View("ViewCase", caseModel);
    }

    public ActionResult SubmitCase(int caseId)
    {
        _caseDataAccess.UpdateCaseStatus(caseId,"Open", _dbConnection);

        return View();
    }

    public ActionResult SaveDraft(int caseId)
    {
        Case caseModel = _caseDataAccess.GetCaseModel(caseId, _dbConnection);

        return View(caseModel);
    }

    public ActionResult DraftCases()
    {
        string accountId = _accountService.GetUserAccountId(_dbConnection);

        Cases casesModel = new Cases();

        casesModel.CasesList.AddRange(_caseDataAccess.GetCases("draft", accountId, _dbConnection));

        return View(casesModel);
    }

    public ActionResult CreateShipment(int caseId)
    {
        Case caseModel = _caseDataAccess.GetCaseModel(caseId, _dbConnection);

        return View(caseModel);
    }

    public ActionResult CancelCase(int caseId)
    {
        _caseDataAccess.UpdateCaseStatus(caseId, "Canceled", _dbConnection);

        Case caseModel = _caseDataAccess.GetCaseModel(caseId, _dbConnection);

        return View();
    }
}
