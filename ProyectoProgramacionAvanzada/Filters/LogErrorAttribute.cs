using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacionAvanzada.Filters
{
    public class LogErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string ruta = HttpContext.Current.Server.MapPath("~/App_Data/log_errores.txt");

            File.AppendAllText(ruta,
                $"[{DateTime.Now}] ERROR: {filterContext.Exception.Message}\n" +
                $"Controlador: {filterContext.RouteData.Values["controller"]}\n" +
                $"Acción: {filterContext.RouteData.Values["action"]}\n" +
                $"-------------------------------\n");

            filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult { ViewName = "Error" };
        }
    }
}
