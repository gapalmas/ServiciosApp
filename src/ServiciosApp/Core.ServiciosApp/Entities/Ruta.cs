using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ServiciosApp.Entities
{
    [Table("Rutas")]
    public class Ruta : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Column("Nombre", TypeName = "nvarchar")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Column("Descripcion", TypeName = "nvarchar")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La distancia es obligatoria")]
        [Range(0.1, 9999.99, ErrorMessage = "La distancia debe ser entre 0.1 y 9999.99 km")]
        [Column("DistanciaKm", TypeName = "decimal")]
        public decimal DistanciaKm { get; set; }

        [Required(ErrorMessage = "El tiempo estimado es obligatorio")]
        [Range(1, 1440, ErrorMessage = "El tiempo estimado debe ser entre 1 y 1440 minutos")]
        [Column("TiempoEstimadoMinutos")]
        public int TiempoEstimadoMinutos { get; set; }

        [Column("Zona", TypeName = "nvarchar")]
        [StringLength(50)]
        public string Zona { get; set; }

        // Navigation Property
        public virtual ICollection<Servicio> Servicios { get; set; }

        public Ruta()
        {
            Servicios = new HashSet<Servicio>();
        }
    }
}
