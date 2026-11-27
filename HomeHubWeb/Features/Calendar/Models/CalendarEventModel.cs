namespace HomeHubWeb.Features.Calendar.Models;

public sealed record CalendarEventModel(
    string Title,
    DateOnly StartDate,
    DateOnly EndDate,
    string Color,
    TimeOnly? StartTime);
