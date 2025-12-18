using System.Web.Mvc;

namespace ProyectoProgramacionAvanzada
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // filters.Add(new LogErrorAttribute()); // desactivado mientras depuramos
        }


    }
}
