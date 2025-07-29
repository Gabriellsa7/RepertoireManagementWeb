
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RepertoireManagementWeb.Models
{
    public class Music
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("pdf_file", TypeName = "varbinary(max)")]
        public byte[]? PdfFile { get; set; }

        [JsonIgnore]
        public virtual ICollection<RepertoireMusic> RepertoireLinks { get; set; } = new HashSet<RepertoireMusic>();
    }
}
