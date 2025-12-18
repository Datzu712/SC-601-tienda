using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;
using ProyectoProgramacionAvanzada.Filters;
using System.Data.Entity;

namespace ProyectoProgramacionAvanzada.Controllers
{
    [AdminOnly]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Dashboard()
        {
            ViewBag.TotalUsuarios = db.Users.Count();
            ViewBag.TotalProductos = db.Productos.Count();
            ViewBag.ProductosBajoInventario = db.Productos.Count(p => p.Inventario < 5);
            ViewBag.TotalOrdenes = db.Ordenes.Count();

            return View();
        }
    }
}
