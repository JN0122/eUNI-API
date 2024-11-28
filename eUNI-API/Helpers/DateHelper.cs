using eUNI_API.Enums;

namespace eUNI_API.Helpers;

public static class DateHelper
{
    public static WeekDay ConvertToWeekDay(DayOfWeek dayOfWeek)
    {
        return (WeekDay)Enum.Parse(typeof(WeekDay), dayOfWeek.ToString());
    }
    
    public static List<DateOnly> CalculateDates(DateOnly yearStart, DateOnly yearEnd, WeekDay classWeekDay, 
        int repeatClassInDays, bool startFirstWeek)
    {
        var dayDifference = (int)classWeekDay - (int)ConvertToWeekDay(yearStart.DayOfWeek);
        var startDate = yearStart.AddDays(dayDifference);
        
        if(!startFirstWeek)
            startDate = startDate.AddDays(7);
        
        if (startDate < yearStart)
            startDate = startDate.AddDays(repeatClassInDays);
        
        var dates = new List<DateOnly> { startDate };
        for (; startDate <= yearEnd; startDate = startDate.AddDays(repeatClassInDays))
            dates.Add(startDate);
        
        return dates;
    }
    
    public static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetWeekStartAndEndDates(int year, int weekNumber)
    {
        var firstDayOfYear = new DateOnly(year, 1, 1);
        var firstWeekStart = firstDayOfYear.AddDays(DayOfWeek.Monday - firstDayOfYear.DayOfWeek);
        var startOfWeek = firstWeekStart.AddDays((weekNumber - 1) * 7);
        
        return (StartOfWeek: startOfWeek, EndOfWeek: startOfWeek.AddDays(6));
    }
    
    public static string GetWeekDay(DateOnly date)
    {
        return ConvertToWeekDay(date.DayOfWeek).ToString();
    }

    public static int GetDayDifference(DateOnly date1, DateOnly date2)
    {
        return (date1.ToDateTime(TimeOnly.MinValue) - date2.ToDateTime(TimeOnly.MinValue)).Days;
    }
}