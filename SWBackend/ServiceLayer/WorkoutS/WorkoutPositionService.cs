using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SWBackend.DataBase;
using SWBackend.DTO.WorkoutDto.WorkoutPositionDto;
using SWBackend.Models.Workout;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;

namespace SWBackend.ServiceLayer.WorkoutS;

public class WorkoutPositionService : IWorkoutPositionService
{
    private readonly IWorkoutPositionRepository _workoutPositionRepository;
    private readonly SwDbContext _context;
    private readonly ILogger<WorkoutPositionService> _logger;

    public WorkoutPositionService(IWorkoutPositionRepository workoutPositionRepository, 
        ILogger<WorkoutPositionService> logger, SwDbContext context)
    {
        _workoutPositionRepository = workoutPositionRepository;
        _logger = logger;
        _context = context;
    }
    public async Task<List<WorkoutPositionResponseDto>> GetAllWorkoutPositions()
    {
        var allWorkoutPositions = await _workoutPositionRepository.GetAllWorkoutPositionsAsync();
        var result = allWorkoutPositions.Select(MapToDto).ToList();
        return result;
    }

    public async Task<WorkoutPositionResponseDto> GetWorkoutPositionByPosId(GetWorkoutPositionByPosIdRequestDto request)
    {
        var result = await _workoutPositionRepository.GetWorkoutPositionByIdAsync(request.PosId);
        if (result == null)
        {
            _logger.LogError("Workout positions not found");
            throw new KeyNotFoundException("Workout position not found");
        }
        return MapToDto(result);
    }

    public async Task<WorkoutPositionResponseDto> GetWorkoutPositionByWorkoutId(GetWorkoutPositionByWorkoutIdRequestDto request)
    {
        var result = await _workoutPositionRepository.GetAllWorkoutPositionsByWorkoutIdAsync(request.WorkId);
        if (result == null)
        {
            _logger.LogError("Workout positions not found");
            throw new KeyNotFoundException("Workout position not found");
        }
        return MapToDto(result);
    }

    public async Task<WorkoutPositionResponseDto> CreateWorkoutPosition(CreateWorkoutPositionRequestDto request)
    {
        var workoutExists = await _context.Workouts.AnyAsync(w => w.Id == request.WorkoutId);
        var positionExists = await _context.Positions.AnyAsync(p => p.Id == request.PositionId);

        if (!workoutExists || !positionExists)
            throw new Exception("Workout o Position non trovati.");
        var newWp = new WorkoutPositionM
        {
            WorkoutId = request.WorkoutId,
            PositionId = request.PositionId,
            MaxShot = request.MaxShot,
        };
        var result = await _workoutPositionRepository.CreateWorkoutPosition(newWp);
        return MapToDto(result);
    }

    private WorkoutPositionResponseDto MapToDto(WorkoutPositionM workoutPosition)
    {
        return new WorkoutPositionResponseDto()
        {
            WorkoutId = workoutPosition.WorkoutId,
            PositionId = workoutPosition.PositionId,
            MaxShot = workoutPosition.MaxShot,
        };
    }
}