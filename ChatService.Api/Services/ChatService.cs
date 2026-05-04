using ChatService.Api.Data;
using ChatService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Api.Services
{
    public class ChatMessageService
    {
        private readonly ChatDbContext _context;

        public ChatMessageService(ChatDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(int senderId, int receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetConversation(int user1, int user2)
        {
            return await _context.Messages
                .Where(m =>
                    (m.SenderId == user1 && m.ReceiverId == user2) ||
                    (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderBy(m => m.SentAt)
                .Take(50) 
                .ToListAsync();
        }
    }
}