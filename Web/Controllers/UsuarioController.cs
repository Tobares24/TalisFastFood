using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Talis_Fast_Food.Utils;
using Web.Security;

namespace Talis_Fast_Food.Controllers
{
    public class UsuarioController : Controller
    {

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.NotificationMessage = TempData["Message"];
            }
            Log.Info("Inicio");

            IServiceUsuario serviceUsuario = new ServiceUsuario();
            IServiceRol serviceRol = new ServiceRol();
            try
            {
                var usuarioList = serviceUsuario.GetUsuarios();
                ViewBag.listaRestaurante = serviceRol.GetRoles();

                return View(usuarioList);
            }

            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult DetalleUsuario(string id)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            USUARIO usuario = null;
            try
            {
                if (string.IsNullOrEmpty(id.Trim()))
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El identificador del usuario es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                usuario = _ServiceUsuario.GetUsuarioById(id.Trim());
                if (usuario is null)
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El usuario no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View(usuario);
        }

        [HttpPost]
        public ActionResult SaveCliente(USUARIO usuario)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            try
            {
                var existeUsuario = _ServiceUsuario.GetUsuarioById(usuario.ID_USUARIO);
                if (existeUsuario != null)
                {
                    ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error",
                        "Este usuario ya existe", SweetAlertMessageType.error);
                    return View("RegistroCliente", usuario);
                }

                if (ModelState.IsValid)
                {
                    USUARIO oUsuarioI = _ServiceUsuario.Save(usuario);
                    TempData["Message"] = SweetAlertHelper.Mensaje("Mensaje", "Usuario registrado exitosamente", Utils.SweetAlertMessageType.success);
                    return RedirectToAction("Index", "InicioSesion");
                }
                else
                {
                    ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error",
                        "Información incompleta", SweetAlertMessageType.error);
                    return View("RegistroCliente", usuario);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Home";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }


        public ActionResult RegistroCliente()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.NotificationMessage = TempData["Message"];
            }
            Log.Info("Inicio");
            return View();
        }

        [HttpPost]
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult SaveUsuario(USUARIO usuario)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            try
            {
                if (ModelState.IsValid)
                {
                    USUARIO oUsuarioI = _ServiceUsuario.Save(usuario);
                    TempData["Message"] = SweetAlertHelper.Mensaje("Mensaje", "Usuario registrado exitosamente", Utils.SweetAlertMessageType.success);
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    ViewBag.IdRestaurante = listaRestaurantes(usuario.ID_RESTAURANTE);
                    ViewBag.IdEstado = listaRoles(usuario.ID_ROL);
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "Información incompleta", SweetAlertMessageType.error);
                    return View("RegistroUsuario", usuario);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Home";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult RegistroUsuario()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.NotificationMessage = TempData["Message"];
            }
            Log.Info("Inicio");
            ViewBag.IdRestaurante = listaRestaurantes();
            ViewBag.IdRol = listaRoles();
            return View();
        }

        [HttpPost]
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Editar(USUARIO usuario)
        {
            IServiceUsuario servicioUsuario = new ServiceUsuario();
            try
            {
                if (ModelState.IsValid)
                {
                    servicioUsuario.Save(usuario);
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Mensaje", "Usuario editado exitosamente", SweetAlertMessageType.success);
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    ViewBag.IdRestaurante = listaRestaurantes(usuario.ID_RESTAURANTE);
                    ViewBag.IdRol = listaRoles(usuario.ID_ROL);
                    return View("EditarUsuario", usuario);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesas";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult EditarUsuario(string id)
        {
            IServiceUsuario serviceUsuario = new ServiceUsuario();
            try
            {
                if (string.IsNullOrEmpty(id.Trim()))
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El identificador del usuario es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var oUsuario = serviceUsuario.GetUsuarioById(id.Trim());
                if (oUsuario is null)
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El usuario no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                ViewBag.IdRestaurante = listaRestaurantes(oUsuario.ID_RESTAURANTE);
                ViewBag.IdRol = listaRoles(oUsuario.ID_ROL);

                return View(oUsuario);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult Mensaje()
        {
            return View();
        }

        private SelectList listaRoles(int idRol = 0)
        {
            IServiceRol servicioRol = new ServiceRol();
            try
            {
                IEnumerable<ROL> listaRoles = servicioRol.GetRoles();
                return new SelectList(listaRoles, "ID_ROL", "TIPO_ROL", idRol);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private SelectList listaRestaurantes(int idRestaurante = 0)
        {
            IServiceRestaurante _ServiceRestaurante = new ServiceRestaurante();
            try
            {
                IEnumerable<RESTAURANTE> listaRestaurantes = _ServiceRestaurante.GetRestaurantes();
                return new SelectList(listaRestaurantes, "ID_RESTAURANTE", "NOMBRE", idRestaurante);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult buscarUsuarioxCedula(string filtro)
        {
            IEnumerable<USUARIO> lista = null;
            IServiceUsuario servicioUsuario = new ServiceUsuario();
            try
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    lista = servicioUsuario.GetUsuarioByCedula(filtro);
                }
                else
                {
                    lista = servicioUsuario.GetUsuarioByCedula(filtro);
                }
                return PartialView("_PartialViewUsuarioAdmin", lista);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Eliminar(string id)
        {
            IServiceUsuario serviceUsuario = new ServiceUsuario();

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El identificador del usuario es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var existeUsuario = serviceUsuario.GetUsuarioById(id);
                if (existeUsuario is null)
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El usuario no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
                return View(existeUsuario);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }


        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult EliminarUsuario(string id)
        {
            IServiceUsuario serviceUsuario = new ServiceUsuario();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El identificador del usuario es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var existeUsuario = serviceUsuario.GetUsuarioById(id);
                if (existeUsuario is null)
                {
                    TempData["Message"] = SweetAlertHelper.Mensaje("Error", "El usuario no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
                serviceUsuario.DeleteUsuario(id);
                TempData["Message"] = SweetAlertHelper.Mensaje("Mensaje", "El usuario ha sido eliminado exitosamente", Utils.SweetAlertMessageType.success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }
    }
}