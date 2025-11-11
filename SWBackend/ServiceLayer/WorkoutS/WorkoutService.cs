using System.Diagnostics;
using System.Security.Claims;
using SWBackend.DTO.WorkoutDto;
using SWBackend.Models.Workout;
using SWBackend.RepositoryLayer.IRepository.User;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;

namespace SWBackend.ServiceLayer.WorkoutS;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<WorkoutM> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkoutService(IWorkoutRepository workoutRepository, IUserRepository userRepository, ILogger<WorkoutM> logger
    , IHttpContextAccessor httpContextAccessor)
    {
        _workoutRepository = workoutRepository;
        _userRepository = userRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    
    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError("Utente non autenticato");
            throw new UnauthorizedAccessException("Utente non autenticato");
        }
        return userId;
    }
    
    public async Task<List<WorkoutResponseDto>> GetAllWorkoutsAsync()
    {
        var allWorkout = await _workoutRepository.GetAllWorkoutsAsync();
        var result = allWorkout.Select(MapToDto).ToList();
        return result;
    }

    public async Task<List<WorkoutResponseDto>> GetWorkoutByNameAsync(GetWorkoutByNameRequestDto request)
    {
        var workoutByName = await _workoutRepository.GetWorkoutByNameAsync(request.WorkoutNameD); 
        var result = workoutByName.Select(MapToDto).ToList();
        if (result.Count == 0)
        {
            _logger.LogError("Workout name not found");
            throw new KeyNotFoundException("Workout name not found");
        }
        return result;
    }

    public async Task<WorkoutResponseDto> GetWorkoutByIdAsync(GetWorkoutByIdRequestDto request)
    {
        var result = await _workoutRepository.GetWorkoutByIdAsync(request.IdD);
        if (result == null)
        {
            _logger.LogError("Utente non trovato");
            throw new KeyNotFoundException("Id not found");
        }
        return MapToDto(result);
    }

    public async Task<WorkoutResponseDto> CreateNewWorkoutAsync(CreateWorkoutRequestDto workout)
    {
        var userId = GetCurrentUserId();
        var newworkout = new WorkoutM
        {
            WorkoutName = workout.WorkoutNameD,
            UserId = int.Parse(userId),
        };
        var result = await _workoutRepository.AddWorkoutAsync(newworkout);
        
        return MapToDto(result);
    }
    
    
    
    private static WorkoutResponseDto MapToDto(WorkoutM workout)
    {
        return new WorkoutResponseDto
        {
            WorkoutId = workout.Id,
            WorkoutName = workout.WorkoutName
        };
    }
}