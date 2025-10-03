using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GOTG.Ronewa.Web.Models
{
    public class IncidentReport
    {
        public int Id { get; set; }

        [Required] // Keeps compatibility with ASP.NET Core MVC model validation
        public required string Title { get; set; }

        [Required]
        public required string Location { get; set; }

        [Required]
        public required string Description { get; set; }

        public IncidentStatus Status { get; set; }

        public DateTime ReportedAt { get; set; }

        // The 'Phone' property was mentioned in the errors, so I've added it
        [Required]
        public required string Phone { get; set; }

        // Foreign Key property for the user who reported the incident
        // This is nullable because a report can be anonymous
        public string? ReportedByUserId { get; set; }

        // Navigation property to the ApplicationUser model
        // This is nullable because the user might not be available
        [ForeignKey("ReportedByUserId")]
        public virtual ApplicationUser? ReportedByUser { get; set; }
    }
}