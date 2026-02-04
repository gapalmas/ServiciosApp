using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ServiciosApp.Entities
{
    [Table("Clientes")]
    public class Cliente : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Column("Nombre", TypeName = "nvarchar")]
        public string Nombre { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        [Column("Direccion", TypeName = "nvarchar")]
        public string Direccion { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        [Column("Telefono", TypeName = "nvarchar")]
        public string Telefono { get; set; }

        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Column("Email", TypeName = "nvarchar")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "El RFC no puede exceder 20 caracteres")]
        [Column("RFC", TypeName = "nvarchar")]
        public string RFC { get; set; }

        // Navigation Property
        public virtual ICollection<Servicio> Servicios { get; set; }

        public Cliente()
        {
            Servicios = new HashSet<Servicio>();
        }
    }
}
