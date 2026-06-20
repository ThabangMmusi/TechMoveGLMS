using System.ComponentModel.DataAnnotations;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.ViewModels
{
    public class DriverScheduleViewModel
    {
        public int Id { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string? DriverPhone { get; set; }
        public string? VehicleNumber { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string PickupLocation { get; set; } = string.Empty;
        public string DeliveryLocation { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TripStatus Status { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsToday => ScheduleDate.Date == DateTime.Today;
        public bool IsUpcoming => ScheduleDate.Date > DateTime.Today;
    }

    public class CreateDriverScheduleViewModel
    {
        [Required]
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; } = string.Empty;

        [Display(Name = "Driver Phone")]
        public string? DriverPhone { get; set; }

        [Display(Name = "Vehicle Number")]
        public string? VehicleNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Pickup Location")]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Delivery Location")]
        public string DeliveryLocation { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; } = DateTime.Today.AddHours(8);

        [Required]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; } = DateTime.Today.AddHours(17);

        [Display(Name = "Status")]
        public TripStatus Status { get; set; } = TripStatus.Scheduled;

        [Display(Name = "Reference Number")]
        public string? ReferenceNumber { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}