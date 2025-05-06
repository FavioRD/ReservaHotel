using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using SistemaReserva.Models;

namespace HotelReservas.Controllers
{
    public class ReservaController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        public ActionResult Index()
        {
            List<Reserva> reservas = new List<Reserva>();
            SqlCommand cmd = new SqlCommand("sp_ListarReservas", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                reservas.Add(new Reserva
                {
                    IdReserva = Convert.ToInt32(rdr["IdReserva"]),
                    FechaEntrada = Convert.ToDateTime(rdr["FechaEntrada"]),
                    FechaSalida = Convert.ToDateTime(rdr["FechaSalida"]),
                    Estado = rdr["Estado"].ToString()
                });
            }
            cn.Close();
            return View(reservas);
        }

        public ActionResult Crear()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("sp_CrearReserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdHuesped", reserva.IdHuesped);
                cmd.Parameters.AddWithValue("@IdHabitacion", reserva.IdHabitacion);
                cmd.Parameters.AddWithValue("@FechaEntrada", reserva.FechaEntrada);
                cmd.Parameters.AddWithValue("@FechaSalida", reserva.FechaSalida);
                cmd.Parameters.AddWithValue("@Estado", reserva.Estado);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                return RedirectToAction("Index");
            }

            CargarCombos();
            return View(reserva);
        }

        private void CargarCombos()
        {
            ViewBag.Huespedes = new SelectList(ObtenerHuespedes(), "IdHuesped", "Nombre");
            ViewBag.Habitaciones = new SelectList(ObtenerHabitacionesDisponibles(), "IdHabitacion", "Numero");
            ViewBag.Estados = new SelectList(new List<string> { "Pendiente", "Confirmada", "Cancelada" });
        }

        private List<Huesped> ObtenerHuespedes()
        {
            List<Huesped> huespedes = new List<Huesped>();
            SqlCommand cmd = new SqlCommand("sp_ListarHuespedes", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                huespedes.Add(new Huesped
                {
                    IdHuesped = Convert.ToInt32(rdr["IdHuesped"]),
                    Nombre = rdr["Nombre"].ToString() + " " + rdr["Apellido"].ToString()
                });
            }
            cn.Close();
            return huespedes;
        }

        private List<Habitacion> ObtenerHabitacionesDisponibles()
        {
            List<Habitacion> habitaciones = new List<Habitacion>();
            SqlCommand cmd = new SqlCommand("sp_ListarHabitaciones", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr["Estado"].ToString() == "Disponible")
                {
                    habitaciones.Add(new Habitacion
                    {
                        IdHabitacion = Convert.ToInt32(rdr["IdHabitacion"]),
                        Numero = rdr["Numero"].ToString()
                    });
                }
            }
            cn.Close();
            return habitaciones;
        }
    }
}
