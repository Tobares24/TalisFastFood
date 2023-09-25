using Infraestructure.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Web.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly int[] allowedroles;
        public CustomAuthorizeAttribute(params int[] roles)
        {
            this.allowedroles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var oUsuario = (USUARIO)httpContext.Session["User"];
            if (oUsuario != null)
            {
                foreach (var rol in allowedroles)
                {
                    if (rol == oUsuario.ID_ROL)
                        return true;
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary
            {
             { "controller", "InicioSesion" },
            { "action", "UnAuthorized" }
                        });
        }
    }
}