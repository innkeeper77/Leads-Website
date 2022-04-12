using Leads_Website.Models;
using LeadsData.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Leads_Website.Controllers
{
    [Route("api/leads")]
    [ApiController]
    public class APIController : ControllerBase
    {

        private readonly LeadsService leadsService;

        #region constructors
        public APIController(LeadsService leadsService)
        {
            this.leadsService = leadsService;
        }
        #endregion constructors

        // GET: api/<APIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "error", "403" };
        }
        // GET: api/<APIController>/<route>
        [HttpGet("{route}")]
        public string Get(string route)
        {
            switch (route)
            {
                case "propertytype":
                    var leads_propertytype = leadsService.Get().OrderBy(l => l.PropertyType).ThenBy(l => l.Project);
                    string jsonString_propertytype = JsonSerializer.Serialize(leads_propertytype);
                    return jsonString_propertytype;
                case "startdate":
                    var leads_startdate = leadsService.Get().OrderBy(l => l.StartDate);
                    string jsonString_startdate = JsonSerializer.Serialize(leads_startdate);
                    return jsonString_startdate;
                case "project":
                    var leads_project = leadsService.Get().OrderBy(l => l.Project);
                    string jsonString_project = JsonSerializer.Serialize(leads_project);
                    return jsonString_project;
                default:
                    return new string("error");
            }
        }

        // POST api/<APIController>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                char delimiter = '-';

                string message = await reader.ReadToEndAsync();
                // Remove first and last quotation marks from line if existing:
                char charQuotation = '"';
                char firstChar = message[0];
                if (charQuotation == firstChar)
                {
                    message = message.Remove(0 , 1);
                    message = message.Remove(message.Length - 1, 1);
                }
                // NO header, so determining delimiter from the first one that shows up. This is delicate and not ideal. If in production this would be a TODO needing an immediate fix, and entered as a bug in the tracking system
                foreach (char c in message)
                {
                    if (c == ',' || c == ' ' || c == '|')
                    {
                        delimiter = c;
                        break;
                    }
                }
                if (delimiter == '-') { return base.BadRequest(); }

                // Split up and create lead using split format. See ImportController.cs
                List<string[]> leadValues = new List<string[]>();
                leadValues.Add(message.Split(delimiter));
                Lead newLead = new Lead(leadValues);
                leadsService.Create(newLead);
                return base.Accepted(); ;
            }
        }
    }
}