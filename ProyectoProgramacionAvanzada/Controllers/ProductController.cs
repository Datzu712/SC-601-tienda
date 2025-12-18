using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;
using System.Data.Entity;
using System.Collections.Generic;
using ProyectoProgramacionAvanzada.Filters;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // LISTA PÚBLICA
        public ActionResult Index()
        {
            var productos = db.Productos
                .Include(p => p.Imagenes)
                .Where(p => p.Activo)
                .ToList();

            return View(productos);
        }

        // DETALLES
        public ActionResult Details(int id)
        {
            var producto = db.Productos
                .Include(p => p.Imagenes)
                .Include(p => p.Resenas)
                .FirstOrDefault(p => p.Id == id);

            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        // CRUD ADMIN
        [AdminOnly]
        public ActionResult AdminList()
        {
            var productos = db.Productos.Include(p => p.Imagenes).ToList();
            return View(productos);
        }

        [AdminOnly]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AdminOnly]
        public ActionResult Create(Producto producto, IEnumerable<HttpPostedFileBase> imagenes)
        {
            if (imagenes == null || imagenes.Count() < 3)
            {
                ViewBag.Error = "Debe subir al menos 3 imágenes.";
                return View(producto);
            }

            db.Productos.Add(producto);
            db.SaveChanges();

            foreach (var archivo in imagenes)
            {
                if (archivo != null && archivo.ContentLength > 0)
                {
                    var img = new ImagenProducto
                    {
                        ProductoId = producto.Id,
                        ContentType = archivo.ContentType
                    };

                    using (var reader = new System.IO.BinaryReader(archivo.InputStream))
                    {
                        img.DatosImagen = reader.ReadBytes(archivo.ContentLength);
                    }

                    db.Imagenes.Add(img);
                }
            }

            db.SaveChanges();
            return RedirectToAction("AdminList");
        }

        [AdminOnly]
        public ActionResult Edit(int id)
        {
            var producto = db.Productos.Find(id);
            return View(producto);
        }

        [HttpPost]
        [AdminOnly]
        public ActionResult Edit(Producto producto)
        {
            db.Entry(producto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AdminList");
        }

        [AdminOnly]
        public ActionResult Delete(int id)
        {
            var p = db.Productos.Find(id);
            db.Productos.Remove(p);
            db.SaveChanges();
            return RedirectToAction("AdminList");
        }

   


    }
}
