using eUNI_API.Enums;

namespace eUNI_API.Helpers;

public static class DateHelper
{
    public static WeekDay ConvertToWeekDay(DayOfWeek dayOfWeek)
    {
        return (WeekDay)Enum.Parse(typeof(WeekDay), dayOfWeek.ToString());
    }
    
    public static DateOnly? CalculateDate(DateOnly yearStart, DateOnly yearEnd, WeekDay classWeekDay, 
        DateOnly startDay, DateOnly endDay, int repeatClassInDays, bool startFirstWeek)
    {
        var date = yearStart.AddDays((int)classWeekDay - (int)DateHelper.ConvertToWeekDay(yearStart.DayOfWeek));
        
        if(!startFirstWeek)
            date = date.AddDays(7);
        
        if (date < yearStart)
            date = date.AddDays(repeatClassInDays);
        
        var dates = new List<DateOnly> { date };
        for (; date <= yearEnd && date <= endDay; date = date.AddDays(repeatClassInDays))
            dates.Add(date);
        
        return dates.Last() < startDay? null : dates.Last();
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
        return DateHelper.ConvertToWeekDay(date.DayOfWeek).ToString();
    }
}