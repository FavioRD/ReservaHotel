using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaReserva.Models
{
    public class Reserva
    {
        public int IdReserva { get; set; }

        [Required(ErrorMessage = "Seleccione un huésped.")]
        public int IdHuesped { get; set; }

        [Required(ErrorMessage = "Seleccione una habitación.")]
        public int IdHabitacion { get; set; }

        [Required(ErrorMessage = "Ingrese fecha de entrada.")]
        public DateTime FechaEntrada { get; set; }

        [Required(ErrorMessage = "Ingrese fecha de salida.")]
        public DateTime FechaSalida { get; set; }

        public string Estado { get; set; } 

        public virtual Huesped Huesped { get; set; }
        public virtual Habitacion Habitacion { get; set; }
    }
}