using Microsoft.EntityFrameworkCore;
using SWBackend.DataBase;
using SWBackend.Models.Workout;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;

namespace SWBackend.RepositoryLayer;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly SWDbContext _context;

    public WorkoutRepository(SWDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<WorkoutM>> GetAllWorkoutsAsync()
    {
        return await _context.Workouts
            .Where(wm => wm.DeleteDate == null)
            .ToListAsync();
    }

    public async Task<WorkoutM?> GetWorkoutByIdAsync(int id)
    {
        return await _context.Workouts.FirstOrDefaultAsync(wm => wm.Id == id);
    }

    public async Task<List<WorkoutM>> GetWorkoutByNameAsync(string name)
    {
        return await _context.Workouts.Where(wm => wm.WorkoutName.Contains(name)).ToListAsync();
    }

    public Task<WorkoutM> GetWorkoutByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<WorkoutM> AddWorkoutAsync(WorkoutM workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();
        return workout;
    }
}