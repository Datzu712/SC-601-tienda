using System.Web.Mvc;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetTestData(string search = "", int page = 1)
        {
            var data = new
            {
                success = true,
                message = "Datos de prueba desde el backend",
                search = search,
                page = page,
                results = new[]
                {
                    new { id = 1, name = "Producto 1", price = 100 },
                    new { id = 2, name = "Producto 2", price = 200 },
                    new { id = 3, name = "Producto 3", price = 300 }
                },
                timestamp = System.DateTime.Now
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult GetErrorData()
        {
            Response.StatusCode = 400;
            return Json(new
            {
                success = false,
                error = "Bad Request",
                message = "skill issue",
                timestamp = System.DateTime.Now
            }, JsonRequestBehavior.AllowGet);
        }
    }
}