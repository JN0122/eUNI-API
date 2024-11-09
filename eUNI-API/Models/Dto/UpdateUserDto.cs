namespace eUNI_API.Models.Dto;

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? NewPassword { get; set; }
}