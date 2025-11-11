using SWBackend.DTO.WorkoutDto.PositionDto;
using SWBackend.Models.Workout;

namespace SWBackend.ServiceLayer.WorkoutS;

public interface IPositionService
{
    Task<List<PositionResponseDto>> GetAllPositionsAsync();
    Task<PositionResponseDto?> GetPositionByIdAsync(GetPositionByIdRequestDto request);
    Task<List<PositionResponseDto>> GetPositionsByNameAsync(GetPositionByNameRequestDto request);
    Task<PositionResponseDto> CreatePositionAsync(CreatePositionRequestDto request);
    Task<bool> DeletePositionAsync(int positionId);
}