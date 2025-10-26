using SWBackend.Models.Workout;

namespace SWBackend.RepositoryLayer.IRepository.WorkoutR;

public interface IWorkoutRepository
{
    Task<List<WorkoutM>> GetAllWorkoutsAsync();
    Task<WorkoutM?> GetWorkoutByIdAsync(int id);
    Task<List<WorkoutM>> GetWorkoutByNameAsync(string name);
    Task<WorkoutM> GetWorkoutByUsernameAsync(string username);
    Task<WorkoutM> AddWorkoutAsync(WorkoutM workout);
}