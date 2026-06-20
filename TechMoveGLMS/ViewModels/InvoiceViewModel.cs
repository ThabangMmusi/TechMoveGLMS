using System.ComponentModel.DataAnnotations;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.ViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int ServiceRequestId { get; set; }
        public string ServiceRequestTitle { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ContractTitle { get; set; } = string.Empty;
        public decimal AmountUSD { get; set; }
        public decimal AmountZAR { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public InvoiceStatus Status { get; set; }
        public string? Notes { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysOverdue { get; set; }
    }

    public class CreateInvoiceViewModel
    {
        [Required]
        public int ServiceRequestId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(30);

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}