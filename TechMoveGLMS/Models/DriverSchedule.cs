using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.Models
{
    public enum TripStatus
    {
        Scheduled = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3,
        Delayed = 4
    }

    public class DriverSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; } = string.Empty;

        [MaxLength(20)]
        [Display(Name = "Driver Phone")]
        public string? DriverPhone { get; set; }

        [MaxLength(20)]
        [Display(Name = "Vehicle Number")]
        public string? VehicleNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Pickup Location")]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Display(Name = "Delivery Location")]
        public string DeliveryLocation { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Status")]
        public TripStatus Status { get; set; } = TripStatus.Scheduled;

        [MaxLength(50)]
        [Display(Name = "Reference Number")]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Updated")]
        public DateTime? UpdatedAt { get; set; }
    }
}