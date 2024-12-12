namespace eUNI_API.Helpers;

public static class OrganizationHelper
{
    public static string GetNextYearName(string oldYearName)
    {
        var oldYear = oldYearName.Split("/");
        return $"{oldYear[1]}/{int.Parse(oldYear[1]) + 1}"; 
    }
    
    public static string GetPrevYearName(string yearName)
    {
        var oldYear = yearName.Split("/");
        return $"{int.Parse(oldYear[0]) - 1}/{oldYear[0]}"; 
    }
}