using CMS.System.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CMS.System.Data.Context
{
    public class MedicaoContext : DbContext
    {
        public MedicaoContext(DbContextOptions<MedicaoContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<DispositivoMedicao> Medicoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Entity<DispositivoMedicao>()
                .ToTable("MEDICOES_TBL");

        }
    }
}
