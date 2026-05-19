using System.ComponentModel.DataAnnotations;

namespace ChatService.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public string Username { get; set; } = string.Empty;

    }
}