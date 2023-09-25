using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Talis_Fast_Food.Utils;
using Web.Security;

namespace Talis_Fast_Food.Controllers
{
    public class MesasController : Controller
    {
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            IEnumerable<MESA> mesaList = null;
            try
            {
                IServiceMesa _ServiceMesa = new ServiceMesa();
                mesaList = _ServiceMesa.GetMesa();
                IServiceRestaurante _ServiceRestaurante = new ServiceRestaurante();
                ViewBag.listaRestaurante = _ServiceRestaurante.GetRestaurantes();
                return View(mesaList);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Mesero)]
        public ActionResult ListaMesas()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            var oUsuario = (USUARIO)Session["User"];
            IEnumerable<MESA> mesaList = null;
            IServiceMesa _ServiceMesa = new ServiceMesa();
            mesaList = _ServiceMesa.GetMesasPorRestaurante(oUsuario.ID_RESTAURANTE).ToList();
            return View(mesaList);
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult DetalleMesas(int? id)
        {
            ServiceMesa _ServiceMesa = new ServiceMesa();
            MESA mesa = null;
            try
            {
                if (id is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "El identificador de la mesa es requerido", Utils.SweetAlertMessageType.warning);
                    return RedirectToAction("Index");
                }

                mesa = _ServiceMesa.GetMesaById(Convert.ToInt32(id));
                if(mesa is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "La mesa no ha sido encontrada", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesa";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View(mesa);
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult EliminarMesa(int? id)
        {
            try
            {
                IServiceMesa serviceMesa = new ServiceMesa();
                MESA oMesa = new MESA();

                if (id is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "El identificador de la mesa es requerido", Utils.SweetAlertMessageType.warning);
                    return RedirectToAction("Index");
                }

                oMesa = serviceMesa.GetMesaById(Convert.ToInt32(id));
                if (oMesa is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "La mesa no ha sido encontrada", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                serviceMesa.DeleteMesa((int)id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesas";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Eliminar(int? id)
        {
            try
            {
                IServiceMesa serviceMesa = new ServiceMesa();

                if (id is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "El identificador de la mesa es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var existeMesa = serviceMesa.GetMesaById((int)id);
                if (existeMesa is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "La mesa no ha sido encontrada", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
                return View(existeMesa);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesas";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Mesero)]
        public ActionResult DetalleMesasM(int? id)
        {
            return DetalleMesas(id);
        }

        public ActionResult Mensaje()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Save(MESA mesa)
        {
            IServiceMesa serviceMesa = new ServiceMesa();
            try
            {
                if (ModelState.IsValid)
                {
                    serviceMesa.Save(mesa);
                }
                else
                {
                    ViewBag.IdRestaurante = listaRestaurantes(mesa.ID_RESTAURANTE);
                    ViewBag.IdEstado = listaEstados(mesa.ID_ESTADO_MESA);
                    return View("AgregarMesas", mesa);
                }

                return RedirectToAction("Mensaje");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesas";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult AgregarMesas()
        {
            ViewBag.IdRestaurante = listaRestaurantes();
            ViewBag.IdEstado = listaEstados();
            return View();
        }

        private SelectList listaEstados(int idEstado = 0)
        {
            IServiceEstadoMesa _ServiceEstado = new ServiceEstadoMesa();
            IEnumerable<Infraestructure.Models.ESTADO_MESA> listaEstados = _ServiceEstado.GetEstadoMesa();
            return new SelectList(listaEstados, "ID_ESTADO_MESA", "ESTADO", idEstado);
        }

        private SelectList listaRestaurantes(int idRestaurante = 0)
        {
            IServiceRestaurante _ServiceRestaurante = new ServiceRestaurante();
            IEnumerable<RESTAURANTE> listaRestaurantes = _ServiceRestaurante.GetRestaurantes();
            return new SelectList(listaRestaurantes, "ID_RESTAURANTE", "NOMBRE", idRestaurante);
        }

        [HttpPost]
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Editar(MESA mesa)
        {
            IServiceMesa service = new ServiceMesa();
            try
            {
                if (ModelState.IsValid)
                {
                    service.Save(mesa);
                }
                else
                {
                    ViewBag.IdRestaurante = listaRestaurantes(mesa.ID_RESTAURANTE);
                    ViewBag.IdEstado = listaEstados(mesa.ID_ESTADO_MESA);
                    return View("AgregarMesas", mesa);
                }
                return RedirectToAction("Mensaje");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesas";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult EditarMesas(int? id)
        {

            IServiceMesa _ServiceMesa = new ServiceMesa();
            MESA oMesa = null;

            try
            {
                if (id is null)
                {
                    return RedirectToAction("Index");
                }

                oMesa = _ServiceMesa.GetMesaById(Convert.ToInt32(id));

                if (oMesa is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "La mesa no ha sido encontrada", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
                ViewBag.IdRestaurante = listaRestaurantes(oMesa.ID_RESTAURANTE);
                ViewBag.IdEstado = listaEstados(oMesa.ID_ESTADO_MESA);

                return View(oMesa);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesas";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        private SelectList listaMeseros(int idUsuario = 0)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            IEnumerable<USUARIO> listaMeseros = _ServiceUsuario.GetMeseros();
            return new SelectList(listaMeseros, "ID_USUARIO", "NOMBRE", idUsuario);
        }

        private SelectList listaOrdenes(int idOrden = 0)
        {
            IServiceOrden _ServiceOrden = new ServiceOrden();
            IEnumerable<ORDEN> listaOrdenes = _ServiceOrden.GetOrden();
            return new SelectList(listaOrdenes, "ID_ORDEN", "ID_ORDEN", idOrden);
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public PartialViewResult mesasxRestaurantes(int? id)
        {
            IEnumerable<MESA> lista = null;
            IServiceMesa _ServicioMesa = new ServiceMesa();

            if (id != null)
            {
                lista = _ServicioMesa.GetMesasPorRestaurante((int)id);
            }
            return PartialView("_PartialViewMesaAdmin", lista);
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult AperturaMesa(int? id)
        {
            IServiceMesa serviceMesa = new ServiceMesa();
            MESA oMesa = null;
            try
            {
                if (id is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "El identificador de la mesa es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                oMesa = serviceMesa.GetMesaById(Convert.ToInt32(id));
                if (oMesa is null)
                {
                    TempData["mensaje"] = SweetAlertHelper.Mensaje("Error", "La mesa no a sido encontrada", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                ViewBag.IdRestaurante = listaRestaurantes(oMesa.ID_RESTAURANTE);
                ViewBag.IdEstado = listaEstados(oMesa.ID_ESTADO_MESA);
                ViewBag.IdOrden = listaOrdenes();
                ViewBag.IdUsuario = listaMeseros();

                return View(oMesa);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["mensaje"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Mesas";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Mesero)]
        public ActionResult AperturaMesaM()
        {
            return View();
        }
    }
}