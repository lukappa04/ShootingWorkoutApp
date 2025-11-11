using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.WorkoutPositionController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("WorkoutPosition")]
    public class GetAllWorkoutPositionController : ControllerBase
    {
        private readonly IWorkoutPositionService _workoutPositionService;

        public GetAllWorkoutPositionController(IWorkoutPositionService workoutPositionService)
        {
            _workoutPositionService = workoutPositionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutPositions()
        {
            return Ok(await _workoutPositionService.GetAllWorkoutPositions());
        }
    }
}
