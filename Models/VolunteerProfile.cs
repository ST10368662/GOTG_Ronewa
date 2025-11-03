using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GOTG.Ronewa.Web.Models
{
    // Define the enum for a clear set of statuses
    public enum VolunteerStatus
    {
        Pending,
        Active,
        Inactive
    }

    public class VolunteerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // This is the single Phone property used in the form and controller
        [Required]
        [StringLength(15)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [StringLength(500)]
        public string? Skills { get; set; }

        // Changed Status from string to the defined enum (default is Pending)
        public VolunteerStatus Status { get; set; } = VolunteerStatus.Pending;
    }
}
