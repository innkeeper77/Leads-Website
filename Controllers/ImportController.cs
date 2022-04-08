using Leads_Website.Models;
using LeadsData.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Leads_Website.Controllers
{
    public class ImportController : Controller
    {
        private readonly LeadsService leadsService;

        public ImportController(LeadsService leadsService)
        {
            this.leadsService = leadsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Import()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SingleFile(IFormFile file)
        {
            //return RedirectToAction("Import");
            if (file == null || file.Length == 0)
            {
                // On errors should give reason for failures, not implemented
                return RedirectToAction("Import");
            }

            // Import file by line
            List<string[]> importValues = new List<string[]>();
            bool checkingHeader = true;
            char delimiter = ',';
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                if (checkingHeader) // Check and change delimiter if needed, break out if invalid header
                {
                    
                    string ?headerLine = reader.ReadLine();
                    if (headerLine != null && headerLine.Length >= 9)
                    {
                        char delimiterCheck = headerLine[8];
                        System.Diagnostics.Debug.WriteLine(headerLine);
                        System.Diagnostics.Debug.WriteLine(delimiterCheck);

                        if (delimiterCheck == ',' || delimiterCheck == ' ' || delimiterCheck == '|')
                        {
                            delimiter = delimiterCheck;
                            string expectedHeader = string.Concat("LastName", delimiter, "FirstName", delimiter, "PropertyType", delimiter, "Project",
                                delimiter, "StartDate", delimiter, "Phone");
                            if (headerLine != expectedHeader) return RedirectToAction("Import");
                            // If delimiter is found and header is as expected, change bool and continue
                            else checkingHeader = false;
                        }
                        else return RedirectToAction("Import");
                    }
                    else return RedirectToAction("Import");

                }
                
                while (!reader.EndOfStream)
                {
                    string ?currentLine = reader.ReadLine();
                    System.Diagnostics.Debug.WriteLine(currentLine);
                    if (currentLine != null)
                    {
                        List<string[]> leadValues = new List<string[]>();
                        leadValues.Add(currentLine.Split(delimiter));
                        Lead newLead = new Lead(leadValues);
                        leadsService.Create(newLead);
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("Import Finished");
            return RedirectToAction("Import");
        }

      
    }
}