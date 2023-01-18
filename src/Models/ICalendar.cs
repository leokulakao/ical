using System.ComponentModel.DataAnnotations;

namespace Ical.Models
{
    public class ICalendar
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Summary { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string? Location { get; set; }
    }
}