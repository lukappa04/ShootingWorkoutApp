using Microsoft.EntityFrameworkCore;
using SWBackend.DataBase;
using SWBackend.Models.Workout;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;

namespace SWBackend.RepositoryLayer;

public class PositionRepository : IPositionRepository
{
    private readonly SwDbContext _context;

    public PositionRepository(SwDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<PositionM>> GetAllPositionsAsync()
    {
        return await _context.Positions.ToListAsync();
    }

    public async Task<PositionM?> GetPositionByIdAsync(int positionId)
    {
        return await _context.Positions.FirstOrDefaultAsync(p => p.Id == positionId);
    }

    public Task<List<PositionM>> GetPositionsByNameAsync(string name)
    {
        return _context.Positions.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }

    public async Task<PositionM> CreatePositionAsync(PositionM position)
    {
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();
        return position;
    }
}