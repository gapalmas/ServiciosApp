using Core.ServiciosApp.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Infrastructure.ServiciosApp.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext() : base(nameof(SqlDbContext)) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Operador> Operadores { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Servicio>()
                .HasRequired(s => s.Cliente)
                .WithMany(c => c.Servicios)
                .HasForeignKey(s => s.ClienteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Servicio>()
                .HasOptional(s => s.Operador)
                .WithMany(o => o.Servicios)
                .HasForeignKey(s => s.OperadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Servicio>()
                .HasOptional(s => s.Ruta)
                .WithMany(r => r.Servicios)
                .HasForeignKey(s => s.RutaId)
                .WillCascadeOnDelete(false);
        }
    }
}
