using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Setup;

public class RootDto
{
    [Required]
    public string Password { get; set; }
}