using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveGLMS.Models
{
    public enum RequestStatus
    {
        [Display(Name = "Pending")]
        Pending = 0,

        [Display(Name = "Approved")]
        Approved = 1,

        [Display(Name = "In Progress")]
        InProgress = 2,

        [Display(Name = "Completed")]
        Completed = 3,

        [Display(Name = "Cancelled")]
        Cancelled = 4
    }

    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Contract")]
        public int ContractId { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Request Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Amount (USD)")]
        public decimal AmountUSD { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Amount (ZAR)")]
        public decimal AmountZAR { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Exchange Rate Used")]
        public decimal ExchangeRate { get; set; }

        [Required]
        [Display(Name = "Request Status")]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Completion Date")]
        public DateTime? CompletedDate { get; set; }

        // Navigation property
        [ForeignKey("ContractId")]
        public virtual Contract? Contract { get; set; }
    }
}