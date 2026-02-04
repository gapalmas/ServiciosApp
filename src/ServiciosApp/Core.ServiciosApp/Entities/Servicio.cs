using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ServiciosApp.Entities
{
    public enum TipoServicio
    {
        Entrega = 1,
        Recoleccion = 2,
        Instalacion = 3,
        Mantenimiento = 4,
        Otro = 5
    }

    public enum EstadoServicio
    {
        Pendiente = 1,
        Asignado = 2,
        EnProceso = 3,
        Completado = 4,
        Cancelado = 5
    }

    [Table("Servicios")]
    public class Servicio : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de servicio es obligatorio")]
        [Column("Tipo")]
        public TipoServicio Tipo { get; set; }

        [Required]
        [Column("Estado")]
        public EstadoServicio Estado { get; set; } = EstadoServicio.Pendiente;

        [Required]
        [Column("FechaSolicitud")]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        [Column("FechaAsignacion")]
        public DateTime? FechaAsignacion { get; set; }

        [Column("FechaCompletado")]
        public DateTime? FechaCompletado { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Column("Descripcion", TypeName = "nvarchar")]
        public string Descripcion { get; set; }

        [StringLength(200, ErrorMessage = "La dirección de origen no puede exceder 200 caracteres")]
        [Column("DireccionOrigen", TypeName = "nvarchar")]
        public string DireccionOrigen { get; set; }

        [StringLength(200, ErrorMessage = "La dirección de destino no puede exceder 200 caracteres")]
        [Column("DireccionDestino", TypeName = "nvarchar")]
        public string DireccionDestino { get; set; }

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Range(0.01, 9999999.99, ErrorMessage = "El costo debe ser mayor a 0")]
        [Column("Costo", TypeName = "decimal")]
        public decimal Costo { get; set; }

        [Column("Observaciones", TypeName = "nvarchar")]
        [StringLength(1000)]
        public string Observaciones { get; set; }

        [Column("Prioridad")]
        public int Prioridad { get; set; } = 3; // 1=Alta, 2=Media, 3=Baja

        // Foreign Keys
        [Required]
        [Column("ClienteId")]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        [Column("OperadorId")]
        [ForeignKey("Operador")]
        public int? OperadorId { get; set; }

        [Column("RutaId")]
        [ForeignKey("Ruta")]
        public int? RutaId { get; set; }

        // Navigation Properties
        public virtual Cliente Cliente { get; set; }
        public virtual Operador Operador { get; set; }
        public virtual Ruta Ruta { get; set; }
    }
}
