using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using SistemaReserva.Models;

namespace HotelReservas.Controllers
{
    public class HuespedController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // Listar Huesped
        public ActionResult Index()
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
                    Nombre = rdr["Nombre"].ToString(),
                    Apellido = rdr["Apellido"].ToString(),
                    Email = rdr["Email"].ToString(),
                    Telefono = rdr["Telefono"].ToString()
                });
            }
            cn.Close();

            return View(huespedes);
        }

        // GET: Crear Huesped
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Huesped huesped)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("sp_CrearHuesped", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", huesped.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", huesped.Apellido);
                cmd.Parameters.AddWithValue("@Email", huesped.Email);
                cmd.Parameters.AddWithValue("@Telefono", huesped.Telefono);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                return RedirectToAction("Index");
            }
            return View(huesped);
        }

        // GET: Editar Huesped
        public ActionResult Editar(int id)
        {
            Huesped huesped = new Huesped();
            SqlCommand cmd = new SqlCommand("sp_ObtenerHuesped", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdHuesped", id);

            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                huesped = new Huesped
                {
                    IdHuesped = Convert.ToInt32(rdr["IdHuesped"]),
                    Nombre = rdr["Nombre"].ToString(),
                    Apellido = rdr["Apellido"].ToString(),
                    Email = rdr["Email"].ToString(),
                    Telefono = rdr["Telefono"].ToString()
                };
            }
            cn.Close();

            return View(huesped);
        }

        [HttpPost]
        public ActionResult Editar(Huesped huesped)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarHuesped", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdHuesped", huesped.IdHuesped);
                cmd.Parameters.AddWithValue("@Nombre", huesped.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", huesped.Apellido);
                cmd.Parameters.AddWithValue("@Email", huesped.Email);
                cmd.Parameters.AddWithValue("@Telefono", huesped.Telefono);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                return RedirectToAction("Index");
            }
            return View(huesped);
        }

        // GET: Eliminar Huesped
        public ActionResult Eliminar(int id)
        {
            SqlCommand cmd = new SqlCommand("sp_EliminarHuesped", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdHuesped", id);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

            return RedirectToAction("Index");
        }
    }
}