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

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Color>().Property(p => p.Hex).IsRequired();
            mb.Entity<Font>().Property(p => p.Name).IsRequired();
        }
    }
}
