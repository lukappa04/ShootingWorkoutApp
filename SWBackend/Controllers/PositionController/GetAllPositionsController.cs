using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.ServiceLayer.IService.IWorkoutService;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.PositionController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Position")]
    public class GetAllPositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public GetAllPositionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositions()
        {
            return Ok(await _positionService.GetAllPositionsAsync());
        }
    }
}
