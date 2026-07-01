using System.ComponentModel.DataAnnotations.Schema;

namespace EF07.Configurations.Entities
{
    [Table("tblTweets")]
    public class Tweet
    {
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string TweetText { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}