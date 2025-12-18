using System;
using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;
using Microsoft.AspNet.Identity;
using ProyectoProgramacionAvanzada.Filters;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult Crear(int productoId, int calificacion, string comentario)
        {
            var resena = new Resena
            {
                ProductoId = productoId,
                UsuarioId = User.Identity.GetUserId(),
                Calificacion = calificacion,
                Comentario = comentario,
                FechaCreacion = DateTime.Now,
                Estado = "Pendiente"
            };

            db.Resenas.Add(resena);
            db.SaveChanges();

            return RedirectToAction("Details", "Product", new { id = productoId });
        }

        [AdminOnly]
        public ActionResult Moderacion()
        {
            var resenas = db.Resenas.Include("Producto").Include("Usuario")
                .Where(r => r.Estado == "Pendiente")
                .ToList();

            return View(resenas);
        }

        [AdminOnly]
        public ActionResult Aprobar(int id)
        {
            var r = db.Resenas.Find(id);
            r.Estado = "Aprobada";
            db.SaveChanges();

            return RedirectToAction("Moderacion");
        }

        [AdminOnly]
        public ActionResult Rechazar(int id)
        {
            var r = db.Resenas.Find(id);
            r.Estado = "Rechazada";
            db.SaveChanges();

            return RedirectToAction("Moderacion");
        }
    }
}
