using MadiffApi.Responses;

namespace MadiffApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardActionsController : ControllerBase
    {
        private readonly IAvailableActionsProvider _availableActionsProvider;
        public CardActionsController(IAvailableActionsProvider availableActionsProvider)
        {
            _availableActionsProvider = availableActionsProvider;
        }

        [HttpPost("available-actions")]
        [ProducesResponseType(typeof(CardActionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CardAction>>> ProcessAvailableCardActions(
            [FromBody] CardActionRequest request)
        {
            var availableActions = await _availableActionsProvider.GetAvailableActions(request.UserId, request.CardNumber);
           
            return Ok(new CardActionResponse(availableActions));
        }
    }
} 