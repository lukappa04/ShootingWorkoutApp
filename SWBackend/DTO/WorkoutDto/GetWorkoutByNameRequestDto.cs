using System.ComponentModel.DataAnnotations;

namespace SWBackend.DTO.WorkoutDto;

public sealed class GetWorkoutByNameRequestDto
{
    [Required]
    public string WorkoutNameD { get; set; }
}