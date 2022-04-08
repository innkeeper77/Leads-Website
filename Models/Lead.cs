using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Leads_Website.Models
{
    public class Lead
    {
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

        // Class members have defaults here due to .Net 6.0 change to warn on non nullable members needing to have non-null value when exiting constuctors
        [BsonId] // Primary Key
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Nullable: must be null to allow mongodb to assign ID itself.

        [BsonElement("LastName")]
        [Required]
        public string LastName { get; set; } = "-1";

        [BsonElement("FirstName")]
        [Required]
        public string FirstName { get; set; } = "-1";

        [BsonElement("Project")]
        [Required]
        public string Project { get; set; } = "-1";

        [BsonElement("PropertyType")]
        [Required]
        public string PropertyType { get; set; } = "-1";

        [BsonElement("StartDate")]
        [Required]
        public DateTime StartDate { get; set; } = new DateTime(1970, 1, 1); // Defaults to unix epoch start time if unspecified

        [BsonElement("Phone")]
        [Required]
        public string Phone { get; set; } = "-1";
    }
}
