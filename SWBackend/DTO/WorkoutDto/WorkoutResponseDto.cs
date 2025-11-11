using System.ComponentModel.DataAnnotations;

namespace SWBackend.DTO.WorkoutDto;

public class WorkoutResponseDto
{
    [Required]
    public string WorkoutName { get; set; }  = string.Empty;
    public int UserId { get; set; }
    public int? WorkoutId { get; set; }
    
}