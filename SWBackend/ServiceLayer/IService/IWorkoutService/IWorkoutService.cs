using SWBackend.DTO.WorkoutDto;

namespace SWBackend.ServiceLayer.WorkoutS;

public interface IWorkoutService
{
    Task<List<WorkoutResponseDto>> GetAllWorkoutsAsync();
    Task<WorkoutResponseDto> GetWorkoutByNameAsync(string workoutName);
    Task<WorkoutResponseDto> GetWorkoutByIdAsync(string workoutId);
    Task<WorkoutResponseDto> CreateNewWorkoutAsync(CreateWorkoutRequestDto workout);
}