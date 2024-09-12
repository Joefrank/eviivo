using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TextMatch.Api.Controllers;
using TextMatch.Domain.Constants;
using TextMatch.Model.Request;
using TextMatch.Services.Abstract;

namespace TextMatch.Controllers
{
    //[ApiController]
    [Route("api/[controller]/[action]")]
    public class TextMatchController : BaseController
    {
        private readonly ILogger<TextMatchController> _logger;
        private readonly ITextMatchingService _matchingService;

        public TextMatchController(ILogger<TextMatchController> logger, ITextMatchingService matchingService)
        {
            _logger = logger;
            _matchingService = matchingService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ComputeTextMatchingPositions([FromBody] TextMatchingRequest stringMatchingRequest)
        {
            _logger.LogInformation($"GetStringMatchingPositions() called with arguments {Environment.NewLine + stringMatchingRequest.MainText} {Environment.NewLine + stringMatchingRequest.SubText}");

            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrors(ModelState));
            }

            var result = _matchingService.GetTextMatchingPositions(stringMatchingRequest.MainText, stringMatchingRequest.SubText);

            if (result.Success && result.FoundPositions.Any())
            {
                var displayMessage = string.Join(",", result.FoundPositions.ToArray());
                return Ok(displayMessage);
            }
            else
            {
                return Ok(GenericMessages.NoOutputMessage);
            }
        }

        [HttpGet("{text}/{subText}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTextMatchingPositions(string text, string subText)
        {
            _logger.LogInformation($"GetStringMatchingPositions() called with arguments {Environment.NewLine + text} {Environment.NewLine + subText}");

            var errorMessage = _matchingService.ValidateTwoStringsForComparison(text, subText);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var result = _matchingService.GetTextMatchingPositions(text, subText);

            if (result.Success && result.FoundPositions.Any())
            {
                var displayMessage = string.Join(",", result.FoundPositions.ToArray());
                return Ok(displayMessage);
            }
            else
            {
                return Ok(GenericMessages.NoOutputMessage);
            }
        }

    }
}
