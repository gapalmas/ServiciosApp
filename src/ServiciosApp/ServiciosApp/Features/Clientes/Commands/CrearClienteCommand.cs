using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ServiciosApp.Features.Clientes.Commands
{
    public class CrearClienteCommand : IRequest<int>
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
