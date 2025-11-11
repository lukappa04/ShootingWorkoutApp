using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.WorkoutDto;
using SWBackend.RepositoryLayer.IRepository.WorkoutR;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.WorkoutController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Workout")]
    public class GetWorkoutByIdController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public GetWorkoutByIdController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWorkoutById(GetWorkoutByIdRequestDto request)
        {
            try
            {
                return Ok(await _workoutService.GetWorkoutByIdAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
