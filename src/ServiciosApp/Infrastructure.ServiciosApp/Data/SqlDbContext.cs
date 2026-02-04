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

            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Operador>().ToTable("Operadores");
            modelBuilder.Entity<Ruta>().ToTable("Rutas");
            modelBuilder.Entity<Servicio>().ToTable("Servicios");

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Operador>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Ruta>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Servicio>()
                .HasKey(s => s.Id);

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
