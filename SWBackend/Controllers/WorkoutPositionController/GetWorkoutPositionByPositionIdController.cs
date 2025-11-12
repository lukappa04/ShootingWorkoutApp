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
    public class GetWorkoutPositionByPositionIdController : ControllerBase
    {
        private readonly IWorkoutPositionService _workoutPositionService;

        public GetWorkoutPositionByPositionIdController(IWorkoutPositionService workoutPositionService)
        {
            _workoutPositionService = workoutPositionService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWorkoutPositionsByPositionId(GetWorkoutPositionByPosIdRequestDto request)
        {
            try
            {
                var result = await _workoutPositionService.GetWorkoutPositionByPosId(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
