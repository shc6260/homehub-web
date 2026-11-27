using HomeHubWeb.Features.Calendar.Models;

namespace HomeHubWeb.Features.Calendar.Services;

public sealed class MockCalendarEventService
{
    public IReadOnlyList<CalendarEventModel> GetEvents(DateOnly today)
    {
        var first = new DateOnly(today.Year, today.Month, 1);
        var last = first.AddDays(DateTime.DaysInMonth(today.Year, today.Month) - 1);

        return
        [
            Create("월간 장보기", first.AddDays(2), "sage", hour: 11),
            Create("치과 정기 검진", first.AddDays(5), "blue", hour: 10),
            Create("도서관 책 반납", first.AddDays(8), "amber"),
            Create("친구들과 점심", first.AddDays(11), "rose", hour: 12),
            Create("가족 여행", first.AddDays(13), "sage", days: 2),
            Create("반려견 예방접종", first.AddDays(18), "blue", hour: 16),
            Create("집 정리", first.AddDays(21), "amber"),
            Create("여행 짐 챙기기", last.AddDays(-1), "lavender", days: 2),
            Create("택배 수령", today, "amber"),
            Create("세탁소 들르기", today, "lavender", hour: 15),
            Create("가족 저녁 식사", today, "rose", hour: 19),
            Create("병원 예약", today.AddDays(1), "blue", hour: 10),
            Create("동네 산책", today.AddDays(2), "sage", hour: 17),
            Create("주말 장보기", today.AddDays(4), "sage", hour: 15)
        ];
    }

    private static CalendarEventModel Create(string title, DateOnly start, string color, int days = 0, int? hour = null) =>
        new(title, start, start.AddDays(days), color, hour is int time ? new TimeOnly(time, 0) : null);
}
