using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Entities.FieldOfStudy;

public class FieldOfStudy
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(20)]
    public string Abbr { get; set; }
    
    public int StudiesCycle { get; set; }
    
    public IEnumerable<FieldOfStudyLog> FieldOfStudyLogs { get; set; }
}