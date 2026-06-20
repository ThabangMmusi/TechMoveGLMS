using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveGLMS.Models
{
    public enum InvoiceStatus
    {
        Draft = 0,
        Sent = 1,
        Paid = 2,
        Overdue = 3,
        Cancelled = 4
    }

    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Service Request")]
        public int ServiceRequestId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; } = string.Empty;

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
        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Paid Date")]
        public DateTime? PaidDate { get; set; }

        [Required]
        [Display(Name = "Status")]
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        [MaxLength(500)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Updated")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        [ForeignKey("ServiceRequestId")]
        public virtual ServiceRequest? ServiceRequest { get; set; }

        // Calculated properties (not stored in database)
        [NotMapped]
        public bool IsOverdue => Status != InvoiceStatus.Paid &&
                                  Status != InvoiceStatus.Cancelled &&
                                  DueDate < DateTime.Today;

        [NotMapped]
        public int DaysOverdue => IsOverdue ? (DateTime.Today - DueDate).Days : 0;
    }
}