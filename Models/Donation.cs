using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GOTG.Ronewa.Web.Models
{
    // ENUM DEFINED HERE
    public enum DonationStatus
    {
        Pledged,
        Collected,
        Distributed
    }

    public class Donation
    {
        public int Id { get; set; }

        [Required]
        public string ResourceType { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public DateTime DonatedAt { get; set; }

        public DonationStatus Status { get; set; } = DonationStatus.Pledged;

        public string? Details { get; set; }

        // Foreign Key to link to the user who made the donation
        public string? DonorId { get; set; }

        // Navigation property to the ApplicationUser
        // This is what was missing and caused the error
        [ForeignKey("DonorId")]
        public virtual ApplicationUser? Donor { get; set; }

        // Property to store the donor's name, especially for anonymous donations
        [Required]
        public string DonorName { get; set; } = string.Empty;
    }
}