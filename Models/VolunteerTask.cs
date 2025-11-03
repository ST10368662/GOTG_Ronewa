using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GOTG.Ronewa.Web.Models
{
    public class VolunteerTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        // This is the property that SQL Server is missing
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? AssignedDate { get; set; }

        public VolunteerTaskStatus Status { get; set; } = VolunteerTaskStatus.Pending;

        public string? AssignedToVolunteerId { get; set; }

        [ForeignKey("AssignedToVolunteerId")]
        public ApplicationUser? AssignedToVolunteer { get; set; }
    }
}
