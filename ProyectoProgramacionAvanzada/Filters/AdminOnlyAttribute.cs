using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacionAvanzada.Filters
{
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.IsInRole("Administrador");
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Product/Index");
        }
    }
}
