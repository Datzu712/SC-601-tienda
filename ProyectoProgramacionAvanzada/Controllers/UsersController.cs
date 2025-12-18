using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProyectoProgramacionAvanzada.Filters;

namespace ProyectoProgramacionAvanzada.Controllers
{
    [AdminOnly]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var usuarios = db.Users.ToList();
            return View(usuarios);
        }

        public ActionResult Activar(string id)
        {
            var user = db.Users.Find(id);
            user.EstaActivo = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Desactivar(string id)
        {
            var user = db.Users.Find(id);
            user.EstaActivo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
