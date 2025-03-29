using Microsoft.EntityFrameworkCore;
using SmartCharging.Domain.Entities;

namespace SmartCharging.Infrastructure.Contexts
{
	public class SmartChargingContext : DbContext
	{
		public DbSet<Group> Groups { get; set; }
		public DbSet<ChargeStation> ChargeStations { get; set; }
		public DbSet<Connector> Connectors { get; set; }

		public SmartChargingContext(DbContextOptions<SmartChargingContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Group Configuration
			modelBuilder.Entity<Group>()
				.HasKey(g => g.Id);

			modelBuilder.Entity<Group>()
				.Property(g => g.Name)
				.IsRequired()
				.HasMaxLength(255);

			modelBuilder.Entity<Group>()
				.Property(g => g.CapacityInAmps)
				.IsRequired();

			modelBuilder.Entity<Group>()
				.HasMany(g => g.ChargeStations)
				.WithOne(cs => cs.Group)
				.HasForeignKey(cs => cs.GroupId)
				.OnDelete(DeleteBehavior.Cascade); // Ensures ChargeStations are deleted with Group

			// ChargeStation Configuration
			modelBuilder.Entity<ChargeStation>()
				.HasKey(cs => cs.Id);

			modelBuilder.Entity<ChargeStation>()
				.Property(cs => cs.Name)
				.IsRequired()
				.HasMaxLength(255);

			modelBuilder.Entity<ChargeStation>()
				.HasMany(cs => cs.Connectors)
				.WithOne(c => c.ChargeStation)
				.HasForeignKey(c => c.ChargeStationId)
				.OnDelete(DeleteBehavior.Cascade); // Ensures Connectors are deleted with ChargeStation

			// Connector Configuration
			modelBuilder.Entity<Connector>()
				.HasKey(c => new { c.ChargeStationId, c.Id }); // Composite key: ChargeStationId + ConnectorId

			modelBuilder.Entity<Connector>()
				.Property(c => c.Id)
				.ValueGeneratedNever(); // Prevent auto-increment since IDs are manual (1-5)

			modelBuilder.Entity<Connector>()
				.Property(c => c.MaxCurrentInAmps)
				.IsRequired();
		}
	}
}
