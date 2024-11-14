namespace eUNI_API.Models.Dto;

public class BasicUserDto
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
}