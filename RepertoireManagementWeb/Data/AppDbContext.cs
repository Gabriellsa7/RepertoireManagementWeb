using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Data
{
    public class AppDbContext : DbContext
    {   
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<Repertoire> Repertoires { get; set; }
        public DbSet<RepertoireMusic> RepertoireMusics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationship Leader (User) -> Band (One-to-Many or One-to-One)
            modelBuilder.Entity<Band>()
                .HasOne(b => b.Leader)
                .WithMany() 
                .HasForeignKey("LeaderId")
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Band>()
            .HasMany(b => b.Members)
            .WithMany(u => u.Bands);


            // Cascate settings in relationship
            modelBuilder.Entity<RepertoireMusic>()
                .HasOne(rm => rm.Repertoire)
                .WithMany(r => r.MusicLinks)
                .HasForeignKey(rm => rm.RepertoireId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RepertoireMusic>()
                .HasOne(rm => rm.Music)
                .WithMany(m => m.RepertoireLinks)
                .HasForeignKey(rm => rm.MusicId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Repertoire>()
            //    .Property(r => r.CreatedAt)
            //    .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
