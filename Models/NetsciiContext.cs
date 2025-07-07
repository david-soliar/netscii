using Microsoft.EntityFrameworkCore;
using netscii.Models.Entities;


namespace netscii.Models
{
    public class NetsciiContext : DbContext
    {
        public NetsciiContext(DbContextOptions<NetsciiContext> options)
            : base(options)
        {
        }

        public DbSet<Color> Colors { get; set; }
        public DbSet<Font> Fonts { get; set; }

        public DbSet<ConversionActivity> ConversionActivities { get; set; }
        public DbSet<ConversionParameters> ConversionParameters { get; set; }
        public DbSet<ConversionAssociation> ConversionAssociations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.Hex).IsRequired();
            });

            modelBuilder.Entity<Font>(entity =>
            {
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.Format).IsRequired();
            });

            modelBuilder.Entity<ConversionActivity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Format).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Width).IsRequired();
                entity.Property(e => e.Height).IsRequired();
                entity.Property(e => e.ProcessingTimeMs).IsRequired();
                entity.Property(e => e.OutputLengthBytes).IsRequired();
            });

            modelBuilder.Entity<ConversionParameters>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Scale).IsRequired();
            });

            modelBuilder.Entity<ConversionAssociation>(entity =>
            {
                entity.HasKey(e => new { e.ConversionActivityId, e.ConversionParametersId });

                entity.HasOne(e => e.ConversionActivity)
                      .WithMany(c => c.ConversionAssociation)
                      .HasForeignKey(e => e.ConversionActivityId);

                entity.HasOne(e => e.ConversionParameters)
                      .WithMany(p => p.ConversionAssociation)
                      .HasForeignKey(e => e.ConversionParametersId);
            });
        }
    }
}
