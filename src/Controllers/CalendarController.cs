using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Ical.Models;
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
    private static readonly List<ICalendar> eventsDB = new List<ICalendar>();

    private readonly ILogger<CalendarController> _logger;

    public CalendarController(ILogger<CalendarController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public IActionResult Get()
    {
        var calendar = new Calendar();
        eventsDB.ForEach(x => calendar.Events.Add(
            new CalendarEvent
            {
                Summary = x.Summary,
                Description = x?.Description,
                Start = new CalDateTime(x.DateStart),
                End = new CalDateTime(x.DateEnd),
                Location = x?.Location
            }
            ));

        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        var contentType = "text/calendar";
        var bytes = Encoding.UTF8.GetBytes(serializedCalendar);

        return File(bytes, contentType, "calendar.ics");
    }

    [HttpPost]
    [Route("/api/add-calendar")]
    public IActionResult AddCalendar([FromBody] ICalendar calendar)
    {
        eventsDB.Add(calendar);

        return Content(JsonSerializer.Serialize(eventsDB), MediaTypeNames.Application.Json, Encoding.UTF8);
    }

    [HttpPost]
    [Route("/api/edit-calendar/{id}")]
    public IActionResult EditCalendar(Guid id, [FromBody] ICalendar calendar)
    {
        ICalendar calendarForEdit = eventsDB.First(x => x.Id == id);

        calendarForEdit.Summary = calendar.Summary;
        calendarForEdit.Description = calendar.Description;
        calendarForEdit.DateStart = calendar.DateStart;
        calendarForEdit.DateEnd = calendar.DateEnd;
        calendarForEdit.Location = calendar.Location;

        return Content(JsonSerializer.Serialize(calendarForEdit), MediaTypeNames.Application.Json, Encoding.UTF8);
    }
}

