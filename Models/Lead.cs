using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Leads_Website.Models
{
    public class Lead
    {
        // Complex type requires a parameterless constructor but this should never be used
        public Lead()
        {
            LastName = "null";
            FirstName = "null";
            PropertyType = "null";
            Project = "null";
            StartDate = new DateTime(1970, 1, 1); // Defaults to unix epoch start time if unspecified
            Phone = "null";
        }
        public Lead(List<string[]> leadValues)
        {
            //leadValues data format is LastName FirstName PropertyType Project StartDate Phone
            // Date must be parsed:
            string[] splitDate = leadValues[0][4].ToString().Split('/');
            int day = int.Parse(splitDate[0]);
            int month = int.Parse(splitDate[1]);
            int year = int.Parse(splitDate[2]);
            DateTime parsedDate = new DateTime(year, month, day);

            LastName = leadValues[0][0].ToString();
            FirstName = leadValues[0][1].ToString();
            PropertyType = leadValues[0][2].ToString();
            Project = leadValues[0][3].ToString();
            StartDate = parsedDate;
            Phone = leadValues[0][5].ToString();
        }

        [BsonId] // Primary Key
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Nullable: must be null to allow mongodb to assign ID itself.

        [BsonElement("LastName")]
        [Required]
        public string LastName { get; set; }

        [BsonElement("FirstName")]
        [Required]
        public string FirstName { get; set; }

        [BsonElement("Project")]
        [Required]
        public string Project { get; set; }

        [BsonElement("PropertyType")]
        [Required]
        public string PropertyType { get; set; }

        [BsonElement("StartDate")]
        [Required]
        public DateTime StartDate { get; set; }

        [BsonElement("Phone")]
        [Required]
        public string Phone { get; set; }
    }
}
