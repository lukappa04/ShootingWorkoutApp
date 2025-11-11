using SWBackend.Models.Workout;

namespace SWBackend.RepositoryLayer.IRepository.WorkoutR;

public interface IPositionRepository
{
    Task<List<PositionM>> GetAllPositionsAsync();
    Task<PositionM?> GetPositionByIdAsync(int positionId);
    Task<List<PositionM>> GetPositionsByNameAsync(string name);
    Task<PositionM> CreatePositionAsync(PositionM position);
}