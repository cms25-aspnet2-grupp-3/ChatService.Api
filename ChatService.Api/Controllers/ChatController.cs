using Microsoft.AspNetCore.Mvc;
using ChatService.Api.Dtos;
using ChatService.Api.Services;

namespace ChatService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatMessageService _chatService;
        private readonly IConfiguration _config;

        public ChatController(ChatMessageService chatService, IConfiguration config)
        {
            _chatService = chatService;
            _config = config;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(
            [FromHeader(Name = "x-api-key")] string? apiKey,
            SendMessageDto dto)
        {
            var validApiKey = _config["ApiKey"];

            if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
                return Unauthorized("Invalid API Key");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.SenderId <= 0 || dto.ReceiverId <= 0)
                return BadRequest("Invalid user");

            await _chatService.SendMessage(dto.SenderId, dto.ReceiverId, dto.Content);

            return Ok(new { message = "Sent" });
        }

        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation(
            [FromHeader(Name = "x-api-key")] string? apiKey,
            int user1, int user2)
        {
            var validApiKey = _config["ApiKey"];

            if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
                return Unauthorized("Invalid API Key");

            var messages = await _chatService.GetConversation(user1, user2);
            return Ok(messages);
        }
    }
}