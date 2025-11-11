
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.WorkoutDto.PositionDto;
using SWBackend.ServiceLayer.IService.IWorkoutService;
using SWBackend.ServiceLayer.WorkoutS;


namespace SWBackend.Controllers.PositionController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Position")]
    public class GetPositionByNameController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public GetPositionByNameController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWorkoutByNameAsync(GetPositionByNameRequestDto request)
        {
            try
            {
                return Ok(await _positionService.GetPositionsByNameAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
