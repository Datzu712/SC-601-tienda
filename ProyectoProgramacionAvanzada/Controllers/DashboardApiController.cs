using System;
using System.Linq;
using System.Web.Http;
using ProyectoProgramacionAvanzada.Models;
using System.Data.Entity;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class DashboardApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/dashboard/estadisticas")]
        public IHttpActionResult GetStats()
        {
            return Ok(new
            {
                TotalUsuarios = db.Users.Count(),
                TotalProductos = db.Productos.Count(),
                BajoInventario = db.Productos.Count(p => p.Inventario < 5),
                TotalOrdenes = db.Ordenes.Count()
            });
        }

        [HttpGet]
        [Route("api/dashboard/ventas")]
        public IHttpActionResult GetVentas()
        {
            var hoy = DateTime.Now.Date;
            var inicio = hoy.AddDays(-6);

            var data = db.Ordenes
                .Where(o => DbFunctions.TruncateTime(o.FechaOrden) >= inicio)
                .GroupBy(o => DbFunctions.TruncateTime(o.FechaOrden))
                .Select(g => new { Fecha = g.Key, Total = g.Sum(x => x.Total) })
                .ToList();

            return Ok(data);
        }
    }
}
