using System;
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

        [HttpGet]
        public JsonResult GetOrderStats(string period = "month")
        {
            var userId = User.Identity.GetUserId();
            var ordenes = db.Ordenes.Where(o => o.UsuarioId == userId).ToList();

            DateTime startDate;
            switch (period.ToLower())
            {
                case "week":
                    startDate = DateTime.Now.AddDays(-7);
                    break;
                case "month":
                    startDate = DateTime.Now.AddMonths(-1);
                    break;
                case "year":
                    startDate = DateTime.Now.AddYears(-1);
                    break;
                default:
                    startDate = DateTime.Now.AddMonths(-1);
                    break;
            }

            var filteredOrders = ordenes.Where(o => o.FechaOrden >= startDate).ToList();
            
            var ordersByStatus = filteredOrders
                .GroupBy(o => o.Estado ?? "Sin estado")
                .Select(g => new { Estado = g.Key, Count = g.Count() })
                .ToList();
            
            var salesByPeriod = filteredOrders
                .GroupBy(o => o.FechaOrden.Date)
                .Select(g => new { Fecha = g.Key.ToString("dd/MM/yyyy"), Total = g.Sum(x => x.Total) })
                .OrderBy(x => x.Fecha)
                .ToList();
            
            var totalSales = filteredOrders.Sum(o => o.Total);
            var totalOrders = filteredOrders.Count;

            return Json(new
            {
                ordersByStatus = ordersByStatus,
                salesByPeriod = salesByPeriod,
                totalSales = totalSales,
                totalOrders = totalOrders,
                period = period
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
