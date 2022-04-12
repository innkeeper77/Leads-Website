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
        var leads = leadsService.Get();
        switch (sortOrder)
        {
            case "PropertyType":
                leads.OrderBy(l => l.LastName).ThenBy(l => l.Project);
                break;
            case "LastName":
                leads.OrderBy(l => l.LastName);
                break;
            case "	StartDate":
                leads.OrderBy(l => l.StartDate);
                break;
            default:
                leads.OrderBy(l => l.Project);
            break;
        }
        return View(leads);
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
