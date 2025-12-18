using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;
using Microsoft.AspNet.Identity;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var carrito = Session["carrito"] as List<CarritoItemViewModel> ?? new List<CarritoItemViewModel>();
            return View(carrito);
        }

        public ActionResult Agregar(int id)
        {
            var carrito = Session["carrito"] as List<CarritoItemViewModel> ?? new List<CarritoItemViewModel>();
            var producto = db.Productos.Find(id);

            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("Index", "Productos");
            }
            
            var productoEnCarrito = carrito.FirstOrDefault(c => c.ProductoId == id);
            if (productoEnCarrito != null)
            {
                productoEnCarrito.Cantidad++;
                productoEnCarrito.SubTotal = productoEnCarrito.Cantidad * productoEnCarrito.Precio;
            }
            else
            {
                carrito.Add(new CarritoItemViewModel
                {
                    ProductoId = producto.Id,
                    NombreProducto = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1,
                    SubTotal = producto.Precio
                });
            }

            Session["carrito"] = carrito;
            return RedirectToAction("Index");
        }

        public ActionResult Remover(int id)
        {
            var carrito = Session["carrito"] as List<CarritoItemViewModel>;
            var item = carrito.FirstOrDefault(c => c.ProductoId == id);
            
            if (item.Cantidad > 1)
            {
                item.Cantidad--;
                item.SubTotal = item.Cantidad * item.Precio;
            }
            else
            {
                carrito.Remove(item);
            }
            
            Session["carrito"] = carrito;

            return RedirectToAction("Index");
        }

        public ActionResult ConfirmarCompra()
        {
            var carrito = Session["carrito"] as List<CarritoItemViewModel>;

            // Validar stock
            foreach (var item in carrito)
            {
                var p = db.Productos.Find(item.ProductoId);
                if (p.Inventario < item.Cantidad)
                {
                    TempData["Error"] = $"No hay suficiente inventario de {p.Nombre}.";
                    return RedirectToAction("Index");
                }
            }

            // Crear Orden
            var orden = new Orden
            {
                UsuarioId = User.Identity.GetUserId(),
                FechaOrden = System.DateTime.Now,
                Total = carrito.Sum(x => x.SubTotal),
                Estado = "Procesada"
            };

            db.Ordenes.Add(orden);
            db.SaveChanges();

            // Guardar detalles
            foreach (var item in carrito)
            {
                db.DetallesOrden.Add(new DetalleOrden
                {
                    OrdenId = orden.Id,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Precio
                });

                // Restar inventario
                var p = db.Productos.Find(item.ProductoId);
                p.Inventario -= item.Cantidad;
            }

            db.SaveChanges();

            Session["carrito"] = null;
            return View("Confirmacion", orden);
        }
    }
}
