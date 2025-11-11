using SWBackend.DTO.WorkoutDto.PositionDto;
using SWBackend.Models.Workout;
using SWBackend.RepositoryLayer.IRepository.User;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;

namespace SWBackend.ServiceLayer.WorkoutS;

public class PositionService : IPositionService
{
    private readonly IPositionRepository _positionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public PositionService(IPositionRepository positionRepository, IUserRepository userRepository, 
        ILogger<PositionService> logger)
    {
        _positionRepository = positionRepository;
        _userRepository = userRepository;
        _logger = logger;
    }
    
    public async Task<List<PositionResponseDto>> GetAllPositionsAsync()
    {
        var allPosition = await _positionRepository.GetAllPositionsAsync();
        var result = allPosition.Select(MapToPosition).ToList();
        return result;
    }

    public async Task<PositionResponseDto?> GetPositionByIdAsync(GetPositionByIdRequestDto request)
    {
        var position = await _positionRepository.GetPositionByIdAsync(request.PositionIdD);
        if (position == null)
        {
            _logger.LogError($"Position not found");
            throw new KeyNotFoundException($"Position not found");
        }
        return MapToPosition(position);
    }

    public async Task<List<PositionResponseDto>> GetPositionsByNameAsync(GetPositionByNameRequestDto request)
    {
        var positionByName = await _positionRepository.GetPositionsByNameAsync(request.NameD);
        var result = positionByName.Select(MapToPosition).ToList();
        if (result.Count == 0)
        {
            _logger.LogError($"Position name not found");
            throw new KeyNotFoundException($"Position name not found");
        }
        return result;
    }

    public async Task<PositionResponseDto> CreatePositionAsync(CreatePositionRequestDto request)
    {
        var newPosition = new PositionM()
        {
            Name = request.NameD,
            Position_X = request.Position_XD,
            Position_Y = request.Position_YD,
        };
        var result = await _positionRepository.CreatePositionAsync(newPosition);
        return MapToPosition(result);
    }

    public Task<bool> DeletePositionAsync(int positionId)
    {
        throw new NotImplementedException();
    }
    private static PositionResponseDto MapToPosition(PositionM position)
    {
        return new PositionResponseDto
        {
            NameD = position.Name,
            Position_XD = position.Position_X,
            Position_YD = position.Position_Y,
        };
        
    }
}
