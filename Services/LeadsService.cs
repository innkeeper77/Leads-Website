using Leads_Website.Models;
using MongoDB.Driver;

namespace LeadsData.Services
{
    public class LeadsService
    {
        private readonly IMongoCollection<Lead> leads;

        public LeadsService(IConfiguration config)
        {
            MongoClient client = new MongoClient(config.GetConnectionString("LeadsDatabase"));
            IMongoDatabase database = client.GetDatabase("LeadsDatabase");
            leads = database.GetCollection<Lead>("Leads");
        }

        public List<Lead> Get()
        {
            return leads.Find(lead => true).ToList();
        }

        public Lead Get(string id)
        {
            return leads.Find(lead => lead.Id == id).FirstOrDefault();
        }

        public Lead Create(Lead lead)
        {
            leads.InsertOne(lead); // Possibility here for null database, should be handled in devops. MongoDB does not appear to have an Exists method. Ideally this method should check
            return lead;
        }

        public void Update(string id, Lead leadIn)
        {
            leads.ReplaceOne(lead => lead.Id == id, leadIn);
        }

        public void Remove(Lead leadIn)
        {
            leads.DeleteOne(lead => lead.Id == leadIn.Id);
        }

        public void Remove(string id)
        {
            leads.DeleteOne(lead => lead.Id == id);
        }

    }
}
