using System.ComponentModel.DataAnnotations.Schema;

namespace EF09.EntityTypesAndMapping.Entities
{

    [NotMapped]
    public class Snapshot
    {
        public DateTime LoadedAt => DateTime.UtcNow;

        // this mean take the first 8 characters of the string representation of the Guid
        public string Version => Guid.NewGuid().ToString()[..8];

        // public string Version => Guid.NewGuid().ToString().Substring(0, 8);
    }
}