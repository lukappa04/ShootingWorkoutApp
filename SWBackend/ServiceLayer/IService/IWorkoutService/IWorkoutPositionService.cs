using SWBackend.DTO.WorkoutDto.WorkoutPositionDto;

namespace SWBackend.ServiceLayer.WorkoutS;

public interface IWorkoutPositionService
{
    Task<List<WorkoutPositionResponseDto>> GetAllWorkoutPositions();
    Task<WorkoutPositionResponseDto> GetWorkoutPositionByPosId(GetWorkoutPositionByPosIdRequestDto request);
    Task<WorkoutPositionResponseDto> GetWorkoutPositionByWorkoutId(GetWorkoutPositionByWorkoutIdRequestDto request);
    Task<WorkoutPositionResponseDto> CreateWorkoutPosition (CreateWorkoutPositionRequestDto request);
}