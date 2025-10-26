using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;

namespace SWBackend.Controllers.WorkoutController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Workout")]
    public class GetAllWorkoutsController : ControllerBase
    {
        private readonly IWorkoutRepository _workoutRepository;

        public GetAllWorkoutsController(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutsAsync()
        {
            return Ok(await _workoutRepository.GetAllWorkoutsAsync());
        }
    }
}
