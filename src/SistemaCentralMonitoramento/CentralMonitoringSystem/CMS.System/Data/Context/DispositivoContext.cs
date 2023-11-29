using CMS.System.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CMS.System.Data.Context
{
    public class DispositivoContext : DbContext
    {
        public DispositivoContext(DbContextOptions<DispositivoContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Dispositivo> Dispositivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");


            modelBuilder.Entity<Dispositivo>()
                .ToTable("DISPOSITIVOS_TBL");

            modelBuilder.Entity<Dispositivo>()
                .Property(c => c.COD_USUARIO_ALTERACAO)
                .HasDefaultValue("SISTEMA");

            modelBuilder.Entity<Dispositivo>()
                .Property(c => c.IND_ATIVO)
                .HasDefaultValue(0);


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }
    }
}
