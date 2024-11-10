using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Entities.OrganizationInfo;

public class Year
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(15)]
    public string Name { get; set; }

    public IEnumerable<OrganizationOfTheYear> OrganizationOfTheYears { get; set; }
}