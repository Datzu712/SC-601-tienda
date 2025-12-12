using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class OrderController : Controller
    {
        private static List<Order> orders = new List<Order>
        {
            new Order { OrderId = 1, UserId = 1, Total = 150.00m, CreatedAt = DateTime.Now.AddDays(-5) },
            new Order { OrderId = 2, UserId = 2, Total = 275.50m, CreatedAt = DateTime.Now.AddDays(-3) },
            new Order { OrderId = 3, UserId = 1, Total = 89.99m, CreatedAt = DateTime.Now.AddDays(-1) }
        };

        public ActionResult Index()
        {
            return View(orders);
        }

        public ActionResult Details(int id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderId = orders.Any() ? orders.Max(o => o.OrderId) + 1 : 1;
                order.CreatedAt = DateTime.Now;
                orders.Add(order);
                return RedirectToAction("Index");
            }
            return View(order);
        }

        public ActionResult Edit(int id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var existingOrder = orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                if (existingOrder != null)
                {
                    existingOrder.UserId = order.UserId;
                    existingOrder.Total = order.Total;
                }
                return RedirectToAction("Index");
            }
            return View(order);
        }

        public ActionResult Delete(int id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == id);
            if (order != null)
            {
                orders.Remove(order);
            }
            return RedirectToAction("Index");
        }
    }
}
