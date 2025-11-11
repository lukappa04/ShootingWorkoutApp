using SWBackend.Models.Workout;

namespace SWBackend.RepositoryLayer.IRepository.WorkoutR;

public interface IWorkoutPositionRepository
{
    Task<List<WorkoutPositionM>> GetAllWorkoutPositionsAsync();
    Task<WorkoutPositionM?> GetWorkoutPositionByIdAsync(int workoutId);
    Task<WorkoutPositionM?> GetAllWorkoutPositionsByWorkoutIdAsync(int workoutId);
    Task<WorkoutPositionM> CreateWorkoutPosition(WorkoutPositionM workoutPositionM);
}