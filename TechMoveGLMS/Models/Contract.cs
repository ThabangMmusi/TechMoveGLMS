using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveGLMS.Models
{
    public enum ContractStatus
    {
        [Display(Name = "Draft")]
        Draft = 0,

        [Display(Name = "Active")]
        Active = 1,

        [Display(Name = "Expired")]
        Expired = 2,

        [Display(Name = "On Hold")]
        OnHold = 3
    }

    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Contract Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Contract Status")]
        public ContractStatus Status { get; set; } = ContractStatus.Draft;

        [Required]
        [MaxLength(50)]
        [Display(Name = "Service Level")]
        public string ServiceLevel { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Signed Agreement")]
        public string? SignedAgreementPath { get; set; }

        [Display(Name = "Original File Name")]
        public string? SignedAgreementFileName { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

        // Calculated properties (not stored in database)
        [NotMapped]
        public int DaysRemaining => (EndDate - DateTime.Today).Days;

        [NotMapped]
        public bool IsExpiringSoon => DaysRemaining > 0 && DaysRemaining <= 30;

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.Today;

        [NotMapped]
        public bool IsValidForService => Status == ContractStatus.Active;
    }
}