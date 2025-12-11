using System.Web.Mvc;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error400()
        {
            return View();
        }

        public ActionResult Error403()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Error500()
        {
            return View();
        }

        public ActionResult General()
        {
            return View();
        }
    }
}
