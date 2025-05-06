using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using SistemaReserva.Models;

namespace HotelReservas.Controllers
{
    public class HabitacionController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // GET: Listar habitaciones
        public ActionResult Index()
        {
            List<Habitacion> habitaciones = new List<Habitacion>();
            SqlCommand cmd = new SqlCommand("sp_ListarHabitaciones", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                habitaciones.Add(new Habitacion
                {
                    IdHabitacion = Convert.ToInt32(rdr["IdHabitacion"]),
                    Numero = rdr["Numero"].ToString(),
                    Tipo = rdr["Tipo"].ToString(),
                    Precio = Convert.ToDecimal(rdr["Precio"]),
                    Estado = rdr["Estado"].ToString()
                });
            }
            cn.Close();

            return View(habitaciones);
        }

        // GET: Filtrar por tipo
        public ActionResult Filtrar(string tipo)
        {
            List<Habitacion> habitaciones = new List<Habitacion>();
            SqlCommand cmd = new SqlCommand("sp_FiltrarHabitacionesPorTipo", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tipo", tipo);

            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                habitaciones.Add(new Habitacion
                {
                    IdHabitacion = Convert.ToInt32(rdr["IdHabitacion"]),
                    Numero = rdr["Numero"].ToString(),
                    Tipo = rdr["Tipo"].ToString(),
                    Precio = Convert.ToDecimal(rdr["Precio"]),
                    Estado = rdr["Estado"].ToString()
                });
            }
            cn.Close();

            return View("Index", habitaciones);
        }

        [HttpPost]
        public ActionResult CambiarEstado(int id, string estado)
        {
            SqlCommand cmd = new SqlCommand("sp_CambiarEstadoHabitacion", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdHabitacion", id);
            cmd.Parameters.AddWithValue("@Estado", estado);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

            return RedirectToAction("Index");
        }

        // GET: Crear nueva habitación
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("sp_CrearHabitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Numero", habitacion.Numero);
                cmd.Parameters.AddWithValue("@Tipo", habitacion.Tipo);
                cmd.Parameters.AddWithValue("@Precio", habitacion.Precio);
                cmd.Parameters.AddWithValue("@Estado", habitacion.Estado ?? "Disponible");

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                return RedirectToAction("Index");
            }
            return View(habitacion);
        }
    }
}