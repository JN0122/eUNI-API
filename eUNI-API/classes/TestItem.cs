using System.ComponentModel.DataAnnotations;

namespace eUNI_API.classes;

public class TestItem
{ 
    [Key]
    public int testId { get; set; } 
    public string testString { get; set; }
}