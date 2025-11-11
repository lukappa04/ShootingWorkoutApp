using Microsoft.EntityFrameworkCore;
using SWBackend.DataBase;
using SWBackend.Models.Workout;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;

namespace SWBackend.RepositoryLayer;

public class WorkoutPositionRepository : IWorkoutPositionRepository
{
    private readonly SwDbContext _context;

    public WorkoutPositionRepository(SwDbContext context)
    {
        _context = context;
    }
    public async Task<List<WorkoutPositionM>> GetAllWorkoutPositionsAsync()
    {
        return await _context.WorkoutPositions.ToListAsync();
    }

    public async Task<WorkoutPositionM?> GetWorkoutPositionByIdAsync(int workoutId)
    {
        return await _context.WorkoutPositions.FirstOrDefaultAsync(wp => wp.Id == workoutId);
    }

    public async Task<WorkoutPositionM?> GetAllWorkoutPositionsByWorkoutIdAsync(int workoutId)
    {
        return await _context.WorkoutPositions.FirstOrDefaultAsync(wp => wp.WorkoutId == workoutId);
    }


    public async Task<WorkoutPositionM> CreateWorkoutPosition(WorkoutPositionM workoutPositionM)
    {
        _context.WorkoutPositions.Add(workoutPositionM);
        await _context.SaveChangesAsync();
        return workoutPositionM;
    }

}