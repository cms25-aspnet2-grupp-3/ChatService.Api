using System.ComponentModel.DataAnnotations;

namespace ChatService.Api.Dtos
{
    public class SendMessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; } = string.Empty;
    }
}