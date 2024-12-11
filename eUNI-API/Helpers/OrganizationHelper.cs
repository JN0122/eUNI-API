namespace eUNI_API.Helpers;

public static class OrganizationHelper
{
    public static string GetNextYearName(string oldYearName)
    {
        var oldYear = oldYearName.Split("/");
        return $"{oldYear[1]}/{int.Parse(oldYear[1]) + 1}"; 
    }
}