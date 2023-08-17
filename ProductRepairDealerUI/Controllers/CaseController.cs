using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductRepairDataAccess.Models;

namespace ProductRepairDealerUI.Controllers
{
    public class CaseController : Controller
    {
        // GET: CaseController
        public ActionResult CreateCase()
        {
            return View();
        }

        public ActionResult NewCase()
        {
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
