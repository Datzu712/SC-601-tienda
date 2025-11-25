using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ProyectoProgramacionAvanzada.Models;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order
        public ActionResult Index()
        {
            var orders = db.Orders
                .Include(o => o.User)
                .ToList();

            return View(orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            var order = db.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Nombre");
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.Fecha = System.DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Nombre", order.UserId);
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Order/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = db.Orders.Find(id);

            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
