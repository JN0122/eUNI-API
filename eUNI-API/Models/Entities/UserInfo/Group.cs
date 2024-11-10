using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Entities.UserInfo;

public class Group
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public string Abbr { get; set; }
    
    public IEnumerable<StudentGroup> StudentGroups { get; set; }
}