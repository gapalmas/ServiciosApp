using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ServiciosApp.Entities
{
    public abstract class BaseEntity
    {
        [Column("Activo")]
        public bool Activo { get; set; } = true;

        [Column("FechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Column("UsuarioRegistro", TypeName = "nvarchar")]
        [StringLength(50)]
        public string UsuarioRegistro { get; set; }

        [Column("UsuarioModificacion", TypeName = "nvarchar")]
        [StringLength(50)]
        public string UsuarioModificacion { get; set; }
    }
}
