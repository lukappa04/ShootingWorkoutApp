namespace SWBackend.DTO.WorkoutDto.WorkoutPositionDto;

public sealed class CreateWorkoutPositionRequestDto
{
    public int WorkoutId { get; set; }
    public int PositionId { get; set; }
    public int MaxShot { get; set; }
}