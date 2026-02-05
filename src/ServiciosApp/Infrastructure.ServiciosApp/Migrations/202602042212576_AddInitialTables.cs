namespace Infrastructure.ServiciosApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Direccion = c.String(maxLength: 200),
                        Telefono = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        RFC = c.String(maxLength: 20),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        UsuarioRegistro = c.String(maxLength: 50),
                        FechaModificacion = c.DateTime(),
                        UsuarioModificacion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Operadores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Licencia = c.String(nullable: false, maxLength: 20),
                        Telefono = c.String(maxLength: 20),
                        Disponible = c.Boolean(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        UsuarioRegistro = c.String(maxLength: 50),
                        FechaModificacion = c.DateTime(),
                        UsuarioModificacion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rutas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        DistanciaKm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TiempoEstimadoMinutos = c.Int(nullable: false),
                        Zona = c.String(maxLength: 50),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        UsuarioRegistro = c.String(maxLength: 50),
                        FechaModificacion = c.DateTime(),
                        UsuarioModificacion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Servicios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        FechaSolicitud = c.DateTime(nullable: false),
                        FechaAsignacion = c.DateTime(),
                        FechaCompletado = c.DateTime(),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        DireccionOrigen = c.String(maxLength: 200),
                        DireccionDestino = c.String(maxLength: 200),
                        Costo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Observaciones = c.String(maxLength: 1000),
                        Prioridad = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        OperadorId = c.Int(),
                        RutaId = c.Int(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        UsuarioRegistro = c.String(maxLength: 50),
                        UsuarioModificacion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.Operadores", t => t.OperadorId)
                .ForeignKey("dbo.Rutas", t => t.RutaId);
            
            CreateIndex("dbo.Servicios", "ClienteId");
            CreateIndex("dbo.Servicios", "OperadorId");
            CreateIndex("dbo.Servicios", "RutaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servicios", "RutaId", "dbo.Rutas");
            DropForeignKey("dbo.Servicios", "OperadorId", "dbo.Operadores");
            DropForeignKey("dbo.Servicios", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.Servicios", new[] { "RutaId" });
            DropIndex("dbo.Servicios", new[] { "OperadorId" });
            DropIndex("dbo.Servicios", new[] { "ClienteId" });
            DropTable("dbo.Servicios");
            DropTable("dbo.Rutas");
            DropTable("dbo.Operadores");
            DropTable("dbo.Clientes");
        }
    }
}
