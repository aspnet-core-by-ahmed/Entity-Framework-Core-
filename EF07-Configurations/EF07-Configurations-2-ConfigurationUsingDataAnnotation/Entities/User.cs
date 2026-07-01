using System.ComponentModel.DataAnnotations.Schema;

namespace EF07.Configurations.Entities
{

    [Table("tblUsers")]
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
    }
}