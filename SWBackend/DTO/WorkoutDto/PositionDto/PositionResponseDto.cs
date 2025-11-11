using System.ComponentModel.DataAnnotations;

namespace SWBackend.DTO.WorkoutDto.PositionDto;

public class PositionResponseDto
{
    [Required]
    public string NameD { get; set; } = String.Empty;
    [Required]
    public double Position_XD { get; set; } 
    [Required]
    public double Position_YD { get; set; }
}