using Infraestructure.Models;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
namespace Web.Security
{
    public class CustomAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if ((USUARIO)filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                 { "controller", "InicioSesion" },
                 { "action", "Index" }
                 });
            }
        }
    }
}