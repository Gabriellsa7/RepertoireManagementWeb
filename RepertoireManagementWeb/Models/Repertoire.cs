
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RepertoireManagementWeb.Models
{
    public class Repertoire
    {
        public Repertoire()
        {
            CreatedAt = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [ForeignKey("BandId")]
        public Guid? BandId { get; set; }

        public virtual Band? Band { get; set; }

        [JsonIgnore]
        public virtual ICollection<RepertoireMusic> MusicLinks { get; set; } = new HashSet<RepertoireMusic>();

        [Required]
        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

    }
}
