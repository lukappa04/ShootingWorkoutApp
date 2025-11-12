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
    public class GetWorkoutPositionByWorkoutIdController : ControllerBase
    {
        private readonly IWorkoutPositionService _workoutPositionService;

        public GetWorkoutPositionByWorkoutIdController(IWorkoutPositionService workoutPositionService)
        {
            _workoutPositionService = workoutPositionService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWorkoutPositionByWorkoutId(GetWorkoutPositionByWorkoutIdRequestDto request)
        {
            try
            {
                var result = await _workoutPositionService.GetWorkoutPositionByWorkoutId(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
