using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.WorkoutDto;
using SWBackend.ServiceLayer.IService.IWorkoutService;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.WorkoutController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Workout")]
    public class CreateNewWorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public CreateNewWorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewWorkout(CreateWorkoutRequestDto request)
        {
            try
            {
                var result = await _workoutService.CreateNewWorkoutAsync(request);

                return Ok("Workout created! Congratulations!");
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create new workout");
            }
        }
    }
}
