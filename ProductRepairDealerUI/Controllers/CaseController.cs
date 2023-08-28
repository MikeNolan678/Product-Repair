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
    private readonly IDataAccessOperations _dataAccess;
    private readonly ICaseDataAccess _caseDataAccess;
    private readonly IItemDataAccess _itemDataAccess;

    public CaseController(
        IAccountService accountService,
        IDataAccessOperations dataAccess, 
        ICaseDataAccess caseDataAccess, 
        IItemDataAccess itemDataAccess)
    {
        _accountService = accountService;
        _dataAccess = dataAccess;
        _caseDataAccess = caseDataAccess;
        _itemDataAccess = itemDataAccess;
    }

    // GET: CaseController
    public async Task<ActionResult> CreateCase()
    {
        string accountId = await _accountService.GetUserAccountId();

        int caseId = await _caseDataAccess.CreateCaseAsync(
                    accountId,
                    IncidentType.RepairRequest,
                    SalesChannel.Dealer,
                    CaseStatus.Draft);

        Case caseModel = new Case
        {
            CaseId = caseId,
            AccountId = accountId
        };

        return View("ViewCase", caseModel);
    }

    // GET: CaseController
    public async Task<ActionResult> ViewCase(int caseId)
    {
        Case caseModel = await _caseDataAccess.GetCaseModel(caseId);

        return View(caseModel);
    }

    public async Task<ActionResult> NewItem(int caseId)
    {
        Case caseModel = await _caseDataAccess.GetCaseModel(caseId);

        return View(caseModel);
    }

    [HttpPost]
    public async Task<ActionResult> AddItem(NewCase newItem)
    {
        // Process the other fields submitted in the form and update the datasources
        await _itemDataAccess.AddItemToCaseAsync(newItem);

        Case caseModel = await _caseDataAccess.GetCaseModel(newItem.CaseId);

        return View("ViewCase", caseModel);
    }

    public async Task<ActionResult> NewItemIssue(int caseId, Guid ItemId)
    {
        Item itemModel = await _itemDataAccess.GetItemModelAsync(ItemId);

        return View(itemModel);
    }

    [HttpPost]
    public async Task<ActionResult> AddItemIssue(NewItemIssue newItemIssue)
    {
        // Process the other fields submitted in the form and update the datasources
        await _itemDataAccess.AddItemIssueToItemAsync(newItemIssue);

        Case caseModel = await _caseDataAccess.GetCaseModel(newItemIssue.CaseId);

        return View("ViewCase", caseModel);
    }

    [HttpPost]
    public async Task<ActionResult> AddCustomerInformation (Case newCustomerCaseModel)
    {  
        if (Request.Form["ReceiveNotification"] == "on")
        {
            newCustomerCaseModel.ReceiveNotification = true;
        }
        else
        {
            newCustomerCaseModel.ReceiveNotification = false;
        }

        await _caseDataAccess.AddCustomerInformationToCase(newCustomerCaseModel);

        Case caseModel = await _caseDataAccess.GetCaseModel(newCustomerCaseModel.CaseId);

        return View("ViewCase", caseModel);
    }

    public async Task<ActionResult> RemoveCustomerInformation (int caseId)
    {

        await _caseDataAccess.RemoveCustomerInformationFromCase(caseId);

        Case caseModel = await _caseDataAccess.GetCaseModel(caseId);

        return View("ViewCase", caseModel);
    }

    public async Task<ActionResult> SubmitCase(int caseId)
    {
        await _caseDataAccess.UpdateCaseStatusAsync(caseId,"Open");

        return View();
    }

    public async Task<ActionResult> SaveDraft(int caseId)
    {
        Case caseModel = await _caseDataAccess.GetCaseModel(caseId);

        return View(caseModel);
    }

    public async Task<ActionResult> DraftCases()
    {
        string accountId = await _accountService.GetUserAccountId();

        Cases casesModel = new Cases();

        casesModel.CasesList.AddRange(await _caseDataAccess.GetCases("draft", accountId));

        return View(casesModel);
    }

    public async Task<ActionResult> CreateShipment(int caseId)
    {
        Case caseModel = await _caseDataAccess.GetCaseModel(caseId);

        return View(caseModel);
    }

    public async Task<ActionResult> CancelCase(int caseId)
    {
        await _caseDataAccess.UpdateCaseStatusAsync(caseId, "Canceled");

        Case caseModel = await _caseDataAccess.GetCaseModel(caseId);

        return View();
    }
}
