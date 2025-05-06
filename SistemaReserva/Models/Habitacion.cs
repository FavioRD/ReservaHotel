using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReserva.Models
{
    public class Habitacion
    {
        public int IdHabitacion { get; set; }
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
    }
}