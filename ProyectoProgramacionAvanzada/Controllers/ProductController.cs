using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;
//using ProyectoProgramacionAvanzada.Service;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class ProductController : Controller
    {
        //private readonly ProductService service = new ProductService();

        public ActionResult Index()
        {
            // var products = service.GetAll();
            return View();
        }

        // public ActionResult Details(int id)
        // {
        //     var product = service.GetById(id);
        //     return View(product);
        // }
        //
        // public ActionResult Create()
        // {
        //     return View();
        // }
        //
        // [HttpPost]
        // public ActionResult Create(Product product)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         service.Create(product);
        //         return RedirectToAction("Index");
        //     }
        //     return View(product);
        // }
        //
        // public ActionResult Edit(int id)
        // {
        //     return View(service.GetById(id));
        // }
        //
        // [HttpPost]
        // public ActionResult Edit(Product product)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         service.Update(product);
        //         return RedirectToAction("Index");
        //     }
        //     return View(product);
        // }
        //
        // public ActionResult Delete(int id)
        // {
        //     return View(service.GetById(id));
        // }
        //
        // [HttpPost, ActionName("Delete")]
        // public ActionResult ConfirmDelete(int id)
        // {
        //     service.Delete(id);
        //     return RedirectToAction("Index");
        // }
    }
}
