using System.ComponentModel.DataAnnotations;
using eUNI_API.Enums;

namespace eUNI_API.Models.Dto.Classes;

public class ClassDto
{
    public int Id { get; set; }
    public int FieldOfStudyLogId { get; set; }
    public string Name { get; set; }
    public string ClassRoom { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public HourDto StartHour { get; set; }
    public HourDto EndHour { get; set; }
    public IEnumerable<DateOnly> Dates { get; set; }
}