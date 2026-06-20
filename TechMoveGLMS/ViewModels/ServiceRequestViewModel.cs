using System.ComponentModel.DataAnnotations;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.ViewModels
{
    public class ServiceRequestViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal AmountUSD { get; set; }
        public decimal AmountZAR { get; set; }
        public decimal ExchangeRate { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string ContractTitle { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public int ContractId { get; set; }
    }

    public class CreateServiceRequestViewModel
    {
        [Required]
        public int ContractId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999.99)]
        [Display(Name = "Amount in USD")]
        public decimal AmountUSD { get; set; }
    }
}
