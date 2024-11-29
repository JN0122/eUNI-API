using System.Collections;

namespace eUNI_API.Models.Dto;

public class UserInfoDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public IEnumerable<int> RepresentativeFieldsOfStudyLogIds { get; set; } = [];
}