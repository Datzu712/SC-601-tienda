using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;
using Microsoft.AspNet.Identity;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var ordenes = db.Ordenes.Where(o => o.UsuarioId == userId).ToList();
            return View(ordenes);
        }

        public ActionResult Details(int id)
        {
            var orden = db.Ordenes
                .Include("Detalles.Producto")
                .FirstOrDefault(o => o.Id == id);

            return View(orden);
        }
    }
}
