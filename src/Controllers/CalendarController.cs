using System.Text;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Ical.Controllers;

[ApiController]
[Route("api/calendar.ics")]
public class CalendarController : ControllerBase
{
    private static readonly IEnumerable<CalendarEvent> events = new CalendarEvent[1]
    {
        new CalendarEvent { Start = new CalDateTime(new DateTime(2023, 11, 1, 15, 0, 0)) }
    }.AsEnumerable();

    private readonly ILogger<CalendarController> _logger;

    public CalendarController(ILogger<CalendarController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public IActionResult Get()
    {
        var calendar = new Calendar();
        events.ToList().ForEach(x => calendar.Events.Add(x));

        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        var contentType = "text/calendar";
        var bytes = Encoding.UTF8.GetBytes(serializedCalendar);

        return File(bytes, contentType, "calendar.ics");
    }
}

