using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.WorkoutDto.WorkoutPositionDto;
using SWBackend.ServiceLayer.IService.IWorkoutService;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.WorkoutPositionController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("WorkoutPosition")]
    public class CreateWorkoutPositionController : ControllerBase
    {
        private readonly IWorkoutPositionService _workoutPositionService;

        public CreateWorkoutPositionController(IWorkoutPositionService workoutPositionService)
        {
            _workoutPositionService = workoutPositionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPosition(CreateWorkoutPositionRequestDto request)
        {
            return Ok(await _workoutPositionService.CreateWorkoutPosition(request));
        }
    }
}
