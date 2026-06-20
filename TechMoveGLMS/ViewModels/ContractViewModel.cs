using System.ComponentModel.DataAnnotations;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.ViewModels
{
    public class ContractViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ContractStatus Status { get; set; }
        public string ServiceLevel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? SignedAgreementFileName { get; set; }

        public int DaysRemaining => (EndDate - DateTime.Today).Days;
        public bool IsExpiringSoon => DaysRemaining > 0 && DaysRemaining <= 30;
        public bool IsExpired => EndDate < DateTime.Today;
    }

    public class CreateContractViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string ClientPhone { get; set; } = string.Empty;

        [Required]
        public string ClientAddress { get; set; } = string.Empty;

        [Required]
        public string ClientRegion { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today.AddYears(1);

        [Required]
        public ContractStatus Status { get; set; } = ContractStatus.Draft;

        [Required]
        public string ServiceLevel { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public IFormFile? SignedAgreement { get; set; }
    }

    public class ContractSearchViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? StartDateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDateTo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDateTo { get; set; }

        public ContractStatus? Status { get; set; }

        public int? ClientId { get; set; }
    }
}