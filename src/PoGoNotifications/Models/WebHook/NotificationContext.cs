using Microsoft.EntityFrameworkCore;

namespace Knapcode.PoGoNotifications.Models
{
    public class NotificationContext : DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
        }

        public DbSet<PokemonEncounter> PokemonEncounters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonEncounter>()
                .Property(x => x.SpawnpointId)
                .IsRequired(false);

            modelBuilder.Entity<PokemonEncounter>()
                .HasKey(x => new { x.EncounterId, x.DisappearTime });
        }
    }
}
