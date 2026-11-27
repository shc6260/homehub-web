using HomeHubWeb.Features.Calendar.Models;

namespace HomeHubWeb.Pages;

public partial class MonthCalendar
{
    private static readonly string[] Weekdays = ["일", "월", "화", "수", "목", "금", "토"];
    private DateOnly today;
    private DateOnly month;
    private DateOnly selected;
    private IReadOnlyList<CalendarEventModel> events = [];
    private readonly List<CalendarWeek> weeks = [];

    protected override void OnInitialized()
    {
        today = DateOnly.FromDateTime(DateTime.Now);
        events = MockEvents.GetEvents(today);
        GoToToday();
    }

    private void GoToToday()
    {
        today = DateOnly.FromDateTime(DateTime.Now);
        SelectDay(today);
    }

    private void MoveMonth(int offset) => SelectDay(month.AddMonths(offset));

    private void SelectDay(DateOnly day)
    {
        selected = day;
        var firstOfMonth = new DateOnly(day.Year, day.Month, 1);
        if (month == firstOfMonth && weeks.Count > 0)
            return;

        month = firstOfMonth;
        BuildWeeks();
    }

    private void BuildWeeks()
    {
        weeks.Clear();
        var firstSunday = month.DayNumber - (int)month.DayOfWeek;
        var lastDay = month.DayNumber + DateTime.DaysInMonth(month.Year, month.Month) - 1;

        for (var sunday = firstSunday; sunday <= lastDay; sunday += 7)
        {
            var saturday = sunday + 6;
            var segments = new List<EventSegment>();
            var rowEnds = new List<int>();
            var weekEvents = events
                .Where(item => item.StartDate.DayNumber <= saturday && item.EndDate.DayNumber >= sunday)
                .OrderBy(item => Math.Max(sunday, item.StartDate.DayNumber))
                .ThenByDescending(item => item.EndDate > item.StartDate)
                .ThenBy(item => item.StartTime);

            foreach (var item in weekEvents)
            {
                var start = Math.Max(sunday, item.StartDate.DayNumber);
                var end = Math.Min(saturday, item.EndDate.DayNumber);

                // Reuse the first row whose previous event ends before this one starts.
                var row = rowEnds.FindIndex(rowEnd => rowEnd < start);
                if (row == -1)
                {
                    row = rowEnds.Count;
                    rowEnds.Add(end);
                }
                else
                {
                    rowEnds[row] = end;
                }

                segments.Add(new(item, start - sunday + 1, end - start + 1, row + 2));
            }

            weeks.Add(new(sunday, Math.Max(2, rowEnds.Count), segments));
        }
    }

    private sealed record CalendarWeek(int Sunday, int EventRows, List<EventSegment> Segments);
    private sealed record EventSegment(CalendarEventModel Event, int Column, int Length, int Row);
}
