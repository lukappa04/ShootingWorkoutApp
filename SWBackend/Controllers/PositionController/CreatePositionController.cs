using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.WorkoutDto.PositionDto;
using SWBackend.Models.Workout;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.PositionController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Position")]
    public class CreatePositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public CreatePositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePosition(CreatePositionRequestDto request)
        {
            try
            {
                await _positionService.CreatePositionAsync(request);
                return Ok("Position created! Congratulations!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
