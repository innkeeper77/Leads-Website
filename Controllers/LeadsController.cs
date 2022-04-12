using LeadsData.Services;
using Leads_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leads_Website.Controllers;


public class LeadsController : Controller
{
    private readonly LeadsService leadsService;

    #region constructors
    public LeadsController(LeadsService leadsService)
    {
        this.leadsService = leadsService;
    }

    #endregion constructors

    // GET: LeadsController
    /*
    public ActionResult Index()
    {
        //return View(leadsService.Get().OrderBy(s => s.PropertyType));
        return View(leadsService.Get());
    }
    */

    // GET: LeadsController with a sorting specified by provided string
    public ActionResult Index(string sortOrder)
    {
        // Convert sort order if descending (causes sorts to be inverted every time selected again)
        ViewBag.LastNameSort = sortOrder == "LastName" ? "LastName_desc" : "LastName";
        ViewBag.PropertyTypeSort = sortOrder == "PropertyType" ? "PropertyType_desc" : "PropertyType";
        ViewBag.StartDateSort = sortOrder == "StartDate" ? "StartDate_desc" : "StartDate";

        var leads = leadsService.Get();
        if (sortOrder == null) { sortOrder = "StartDate"; };
        switch (sortOrder)
        {
            case "PropertyType":
                return View(leadsService.Get().OrderBy(l => l.PropertyType).ThenBy(l => l.Project));
            case "PropertyTypeDesc":
                return View(leadsService.Get().OrderByDescending(l => l.PropertyType).ThenBy(l => l.Project));
            case "LastName":
                return View(leadsService.Get().OrderBy(s => s.LastName));
            case "LastName_desc":
                return View(leadsService.Get().OrderByDescending(s => s.LastName));
            case "StartDate":
                return View(leadsService.Get().OrderBy(s => s.StartDate));
            case "StartDate_desc":
                return View(leadsService.Get().OrderByDescending(s => s.StartDate));
            default:
                return View(leadsService.Get());
        }
    }

    // GET: LeadsController/Details/5
    public ActionResult Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lead = leadsService.Get(id);
        if (lead == null)
        {
            return NotFound();
        }
        return View(lead);
    }

    // GET: LeadsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: LeadsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Lead lead)
    {
        try
        {
            leadsService.Create(lead);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(lead);
        }
    }

    // GET: LeadsController/Edit/5
    public ActionResult Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var lead = leadsService.Get(id);
        if (lead == null)
        {
            return NotFound();
        }
        return View(lead);
    }

    // POST: LeadsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(string id, Lead lead)
    {
        if (id != lead.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            leadsService.Update(id, lead);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(lead);
        }
    }

    // GET: LeadsController/Delete/5
    public ActionResult Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lead = leadsService.Get(id);
        if (lead == null)
        {
            return NotFound();
        }
        return View(lead);
    }

    // POST: LeadsController/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(string id)
    {
        try
        {
            var lead = leadsService.Get(id);

            if (lead == null)
            {
                return NotFound();
            }
            else
            {
                #pragma warning disable CS8604 // Possible null reference argument.
                leadsService.Remove(lead.Id); // catch below handles if Id is null, as well as above if, so warning may be supressed.
                #pragma warning restore CS8604 // Possible null reference argument.
                return RedirectToAction(nameof(Index));
            }
        }
        catch
        {
            return View();
        }
    }
}
