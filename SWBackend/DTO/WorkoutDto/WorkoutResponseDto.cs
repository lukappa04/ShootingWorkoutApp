using System.ComponentModel.DataAnnotations;

namespace SWBackend.DTO.WorkoutDto;

public class WorkoutResponseDto
{
    [Required]
    public string WorkoutName { get; set; }
    public string UserId { get; set; } = string.Empty; 
    
}