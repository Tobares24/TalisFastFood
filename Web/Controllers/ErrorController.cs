using System;
using System.Web;
using System.Web.Mvc;
using Web.Security;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Index(Exception exception)
        {
            string redirect = "Home";
            string redirectAction = "Index";

            HttpException httpException = exception as HttpException;
            int error = httpException != null ? httpException.GetHttpCode() : 0;
            switch (error)
            {
                case 400:
                    ViewBag.Title = "Solicitud incorrecta";
                    ViewBag.Description = "El servidor no pudo interpretar la solicitud dada una sintaxis inválida.";
                    break;

                case 401:
                    ViewBag.Title = "No autorizado";
                    ViewBag.Description = "Es necesario autenticar el usuario para obtener la respuesta solicitada";
                    break;
                case 403:
                    ViewBag.Title = "Acceso restringido";
                    ViewBag.Description = "El usuario no posee los permisos necesarios para el contenido";
                    break;
                default:
                    ViewBag.Title = error + " Error";
                    ViewBag.Description = exception.Message;
                    break;
            }
            ViewBag.redirect = redirect;
            ViewBag.redirectAction = redirectAction;

            return View();
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult NotFound()
        {
            return View();
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Default()
        {
            string redirect = "Home";
            string redirectAction = "Index";
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Title = "Error";
                ViewBag.Description = TempData["Message"];
                if (TempData.ContainsKey("Redirect"))
                {
                    redirect = (string)TempData["Redirect"];
                }
                if (TempData.ContainsKey("Redirect-Action"))
                {
                    redirectAction = (string)TempData["Redirect-Action"];
                }
                ViewBag.redirect = redirect;
                ViewBag.redirectAction = redirectAction;
                return View("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}