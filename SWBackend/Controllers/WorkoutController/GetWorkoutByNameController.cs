using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.WorkoutDto;
using SWBackend.Models.Workout;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.WorkoutController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Workout")]
    public class GetWorkoutByNameController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public GetWorkoutByNameController(IWorkoutService workoutService)
        {
         _workoutService = workoutService;   
        }

        [HttpPost]
        public async Task<IActionResult> GetWorkoutByNameAsync(GetWorkoutByNameRequestDto request)
        {
            try
            {
                return Ok(await _workoutService.GetWorkoutByNameAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
