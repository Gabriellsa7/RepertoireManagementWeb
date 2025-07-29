
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepertoireManagementWeb.Models
{
    [Table("repertoire_music")]
    public class RepertoireMusic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("order_in_repertoire")]
        public int OrderInRepertoire { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [ForeignKey("RepertoireId")]
        public Guid? RepertoireId { get; set; }

        public virtual Repertoire? Repertoire { get; set; }

        [ForeignKey("MusicId")]
        public Guid? MusicId { get; set; }

        public virtual Music? Music { get; set; }
    }
}
