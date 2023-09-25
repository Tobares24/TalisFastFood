using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Reflection;
using System.Web.Mvc;
using Talis_Fast_Food.Utils;

namespace Talis_Fast_Food.Controllers
{
    public class InicioSesionController : Controller
    {
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            Log.Info("Inicio");
            return View();
        }

        [HttpPost]
        public ActionResult Login(USUARIO usuario)
        {
            IServiceUsuario servicioUsuario = new ServiceUsuario();
            USUARIO oUsuario = null;
            try
            {
                ModelState.Remove("NOMBRE");
                ModelState.Remove("PRIMER_APELLIDO");
                ModelState.Remove("SEGUNDO_APELLIDO");
                ModelState.Remove("ID_ROL");

                if (ModelState.IsValid)
                {

                    oUsuario = servicioUsuario.GetUsuario(usuario.EMAIL, usuario.CLAVE);

                    if (oUsuario != null)
                    {
                        Session["User"] = oUsuario;
                        Log.Info($"Accede {oUsuario.NOMBRE} {oUsuario.PRIMER_APELLIDO} {oUsuario.SEGUNDO_APELLIDO}" +
                            $"con el Rol {oUsuario.ROL.ID_ROL}");
                        TempData["mensaje"] = SweetAlertHelper.Mensaje("Inicio de Sesión", "Usuario autenticado", Utils.SweetAlertMessageType.success);
                        if (oUsuario.ID_ROL == (int)Roles.Administrador)
                        {
                            return RedirectToAction("IndexUsers", "Home");
                        }
                        else if (oUsuario.ID_ROL == (int)Roles.Cliente)
                        {
                            return RedirectToAction("ListaProductos", "Orden");
                        }
                        else
                        {
                            return RedirectToAction("ListaMesas", "Mesas");
                        }
                    }
                    else
                    {
                        Log.Warn($"Intento de inicio de sesión {usuario.EMAIL}");
                        TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "Email y/o contraseña inválidos", Utils.SweetAlertMessageType.error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                return RedirectToAction("Default", "Error");
            }

            return RedirectToAction("Index", "InicioSesion");
        }

        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "No autorizado";
            if (Session["User"] != null)
            {
                USUARIO usuario = Session["User"] as USUARIO;
                Log.Warn($"No autorizado {usuario.EMAIL}");
            }
            return View();
        }

        public ActionResult LogOut()
        {
            try
            {
                Session["User"] = null;
                TempData["mensaje"] = SweetAlertHelper.Mensaje("Cerrar Sesión", "Sesión cerrada con éxito", Utils.SweetAlertMessageType.success);
                return RedirectToAction("Index", "InicioSesion");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                return RedirectToAction("Default", "Error");
            }
        }
    }
}
