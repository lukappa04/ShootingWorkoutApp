using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.WorkoutDto.PositionDto;
using SWBackend.ServiceLayer.WorkoutS;

namespace SWBackend.Controllers.PositionController
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Position")]
    public class GetPositionByIdController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public GetPositionByIdController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpPost]
        public async Task<IActionResult> GetPositionById(GetPositionByIdRequestDto request)
        {
            try
            {
                return Ok(await _positionService.GetPositionByIdAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
