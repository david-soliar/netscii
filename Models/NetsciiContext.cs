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

        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<SuggestionCategory> SuggestionCategories { get; set; }
        public DbSet<SuggestionCategoryAssociation> SuggestionCategoryAssociations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

                entity.HasOne(e => e.ConversionParameters)
                      .WithMany(p => p.Activities)
                      .HasForeignKey(e => e.ConversionParametersId);
            });

            modelBuilder.Entity<ConversionParameters>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Scale).IsRequired();
            });


            modelBuilder.Entity<Suggestion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<SuggestionCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CategoryName).IsRequired();
            });

            modelBuilder.Entity<SuggestionCategoryAssociation>(entity =>
            {
                entity.HasKey(e => new { e.SuggestionId, e.SuggestionCategoryId });

                entity.HasOne(e => e.Suggestion)
                      .WithMany(s => s.SuggestionCategoryAssociations)
                      .HasForeignKey(e => e.SuggestionId);

                entity.HasOne(e => e.SuggestionCategory)
                      .WithMany(c => c.SuggestionAssociations)
                      .HasForeignKey(e => e.SuggestionCategoryId);
            });
        }
    }
}
