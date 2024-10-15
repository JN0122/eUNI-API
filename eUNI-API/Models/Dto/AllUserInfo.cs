namespace eUNI_API.Models.Dto;

public class AllUserInfo
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public string? EmploymentUnit { get; set; }
    public List<string>? FieldsOfStudy { get; set; }
}