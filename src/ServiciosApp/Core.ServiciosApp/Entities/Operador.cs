using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ServiciosApp.Entities
{
    [Table("Operadores")]
    public class Operador : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Column("Nombre", TypeName = "nvarchar")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La licencia es obligatoria")]
        [StringLength(20, ErrorMessage = "La licencia no puede exceder 20 caracteres")]
        [Column("Licencia", TypeName = "nvarchar")]
        public string Licencia { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        [Column("Telefono", TypeName = "nvarchar")]
        public string Telefono { get; set; }

        [Column("Disponible")]
        public bool Disponible { get; set; } = true;

        // Navigation Property
        public virtual ICollection<Servicio> Servicios { get; set; }

        public Operador()
        {
            Servicios = new HashSet<Servicio>();
        }
    }
}
