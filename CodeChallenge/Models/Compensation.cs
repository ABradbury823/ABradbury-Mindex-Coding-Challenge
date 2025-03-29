using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key]
        [ForeignKey("Employee")]
        public String EmployeeId { get; set; }

        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }

        // navigation property
        [JsonIgnore]    // do not serialize to avoid infinite loops
        public Employee Employee { get; set; }
    }
}