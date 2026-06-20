using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.Models
{
    public enum DriverStatus
    {
        Available = 0,
        OnRoute = 1,
        OffDuty = 2
    }

    public class Driver
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? VehicleNumber { get; set; }

        public DriverStatus Status { get; set; } = DriverStatus.Available;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}