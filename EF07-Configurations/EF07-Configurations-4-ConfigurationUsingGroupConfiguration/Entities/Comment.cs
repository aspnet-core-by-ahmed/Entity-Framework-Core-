using System.ComponentModel.DataAnnotations.Schema;

namespace EF07.Configurations.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }
        public string CommentText { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}