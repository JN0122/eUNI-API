using eUNI_API.Enums;

namespace eUNI_API.Helpers;

public static class ConvertDay
{
    public static WeekDay ToWeekDay(DayOfWeek dayOfWeek)
    {
        return (WeekDay)Enum.Parse(typeof(WeekDay), dayOfWeek.ToString());
    }
}