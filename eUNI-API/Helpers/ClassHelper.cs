namespace eUNI_API.Helpers;

public static class ClassHelper
{
    public static string GetClassWithGroup(string className, string groupName)
    {
        return $"{className} ({groupName})";
    }
}