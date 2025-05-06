using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaReserva.Models
{
    public class Huesped
    {
        public int IdHuesped { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        public string Email { get; set; }

        public string Telefono { get; set; }
        public string NombreCompleto
        {
            get { return $"{Nombre} {Apellido}"; }
        }
    }
}