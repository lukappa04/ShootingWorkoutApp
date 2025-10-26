using System.ComponentModel.DataAnnotations;

namespace SWBackend.DTO.WorkoutDto;

public sealed class GetWorkoutByUsernameRequestDto
{
    [Required]
    public string UsernameD { get; set; }
}