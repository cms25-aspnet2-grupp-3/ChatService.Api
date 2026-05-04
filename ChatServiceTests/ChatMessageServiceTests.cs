using System;
using System.Threading.Tasks;
using ChatService.Api.Data;
using ChatService.Api.Models;
using ChatService.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace ChatService.Tests
{
    public class ChatMessageServiceTests
    {
        private ChatDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ChatDbContext(options);
        }

        [Fact]
        public async Task SendMessage_ShouldSaveMessage()
        {
            var context = GetDbContext();
            var service = new ChatMessageService(context);

            await service.SendMessage(1, 2, "Hello");

            var count = await context.Messages.CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetConversation_ShouldReturnMessages()
        {
            var context = GetDbContext();

            context.Messages.Add(new Message
            {
                SenderId = 1,
                ReceiverId = 2,
                Content = "Hi"
            });

            context.Messages.Add(new Message
            {
                SenderId = 2,
                ReceiverId = 1,
                Content = "Hello back"
            });

            await context.SaveChangesAsync();

            var service = new ChatMessageService(context);

            var result = await service.GetConversation(1, 2);

            Assert.Equal(2, result.Count);
        }
    }
}