using MeetingHelper.DTOs;
using MeetingHelper.Services;
using Microsoft.AspNetCore.Mvc;


namespace MeetingHelper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("summarizeText")]
        public async Task<IActionResult> GetSummeryFromText([FromBody] TextPromptRequestDTO prompt)
        {
            try
            {
                var result = await _aiService.GetSummeryFromText(prompt.Content);
                var responsePayload = new AiResponseDTO(result);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("summarizeAudio")]
        [RequestSizeLimit(200_000_000)] // Tillåter ~200 MB i Kestrel
        [RequestFormLimits(MultipartBodyLengthLimit = 200_000_000)] // Tillåter ~200 MB i formuläret
        public async Task<IActionResult> GetSummeryFromAudio(IFormFile file)
        {
            var allowedAudioTypes = new[] { "audio/mpeg", "audio/wav", "audio/mp4", "audio/flac", "audio/webm", "audio/aac" };

            if (!allowedAudioTypes.Contains(file.ContentType))
            {
                return BadRequest("Only MP3, WAV, MP4, FLAC, WEBM and AAC are supported.");
            }

            try
            {
                var result = await _aiService.GetSummeryFromAudio(file);
                var responsePayload = new AiResponseDTO(result);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return (StatusCode(500, ex.Message));
            }
        }

        [HttpPost("agenda")]
        public async Task<IActionResult> GetAgendaFromPrompt(AgendaRequestDTO prompt)
        {
            try
            {
                var result = await _aiService.CreateMeetingAgenda(prompt.Topic, prompt.Duration, prompt.Attendees);
                var responsePayload = new AiResponseDTO(result);
                return Ok(responsePayload);
            }
            catch (Exception ex)
            {
                return (StatusCode(500, ex.Message));
            }
        }

        [HttpPost("invitation")]
        public async Task<IActionResult> GetInvitationFromPrompt(InvitationRequestDTO prompt)
        {
            try
            {
                var result = await _aiService.CreateInvitationMessage(prompt.MeetingTitle, prompt.DateAndTime, prompt.Location, prompt.Purpose);
                var responsePayload = new AiResponseDTO(result);
                return Ok(responsePayload);

            }
            catch (Exception ex)
            {
                return (StatusCode(500, ex.Message));
            }
        }
    }
}


