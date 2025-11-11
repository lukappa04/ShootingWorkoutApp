using SWBackend.DTO.WorkoutDto;

namespace SWBackend.ServiceLayer.WorkoutS;

public interface IWorkoutService
{
    Task<List<WorkoutResponseDto>> GetAllWorkoutsAsync();
    Task<List<WorkoutResponseDto>> GetWorkoutByNameAsync(GetWorkoutByNameRequestDto request);
    Task<WorkoutResponseDto> GetWorkoutByIdAsync(GetWorkoutByIdRequestDto request);
    Task<WorkoutResponseDto> CreateNewWorkoutAsync(CreateWorkoutRequestDto workout);
}