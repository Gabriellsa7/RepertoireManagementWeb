using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RepertoireManagementWeb.Models
{
    public class Band
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [ForeignKey("LeaderId")]
        public Guid? LeaderId { get; set; }

        public virtual User Leader { get; set; }

        // Tabela de junção automática em EF Core 5+
        [JsonIgnore]
        public virtual ICollection<User> Members { get; set; } = new HashSet<User>();

        [JsonIgnore]
        public virtual ICollection<Repertoire> Repertoires { get; set; } = new HashSet<Repertoire>();
    }
}
