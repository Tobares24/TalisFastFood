using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Talis_Fast_Food.Utils;
using Web.Security;

namespace Web.Controllers
{
    public class FacturaController : Controller
    {
        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Mesero, (int)Roles.Administrador)]
        public ActionResult Index(int? id)
        {
            if (TempData.ContainsKey("NotificationMessage"))
            {
                ViewBag.NotificationMessage = TempData["NotificationMessage"];
            }

            ORDEN orden = null;
            USUARIO oUsuario = (USUARIO)Session["User"];
            IServiceOrden serviceOrden = new ServiceOrden();
            IServiceDetalleOrden detalleOrden = new ServiceDetalleOrden();

            if (id == null)
            {
                TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El identificador de la orden es requerido es requerido", SweetAlertMessageType.error);
                return RedirectToAction("DetalleMiOrden");
            }
            try
            {
                orden = serviceOrden.GetOrdenById((int)id);
                ViewBag.DetalleOrden = detalleOrden.GetDetalleOrden().Where(x => x.ID_ORDEN == orden.ID_ORDEN).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Factura";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View();
        }

        public ActionResult FacturaUsuario()
        {
            USUARIO oUsuario = (USUARIO)Session["User"];
            IServiceFactura serviceFactura = new ServiceFactura();
            List<FACTURA> listaFactura = new List<FACTURA>();
            try
            {
                if (oUsuario != null)
                {
                    listaFactura =  serviceFactura.GetFacturaUsuario(oUsuario.ID_USUARIO).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Factura";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View(listaFactura);
        }

        public ActionResult DetalleOrdenUsuario(int? idFactura)
        {
            IServiceOrden serviceOrden = new ServiceOrden();
            IServiceFactura serviceFactura = new ServiceFactura();
            ORDEN orden = null;
            
            if(idFactura is null)
            {
                TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El identificador de la factura es requerido", SweetAlertMessageType.error);
                return RedirectToAction("DetalleMiOrden");
            }

            try
            {
                var existeFactura = serviceFactura.GetFacturaById((int)idFactura);
                if(existeFactura is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "La factura no ha sido encontrada", SweetAlertMessageType.error);
                    return RedirectToAction("DetalleMiOrden");
                }
                orden =  serviceOrden.GetOrdenByFactura((int)idFactura);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Factura";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View(orden);
        }

        [CustomAuthorize((int)Roles.Mesero, (int)Roles.Administrador)]
        public ActionResult Facturas()
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            List<FACTURA> factura = new List<FACTURA>();

            try
            {
                factura = serviceFactura.GetFacturas().ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Factura";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View(factura);
        }

        [HttpPost]
        [CustomAuthorize((int)Roles.Mesero, (int)Roles.Administrador, (int)Roles.Cliente)]
        public ActionResult Save(FACTURA factura)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            IServiceMesa serviceMesa = new ServiceMesa();
            USUARIO oUsuario = (USUARIO)Session["User"];
            IServiceOrden serviceOrden = new ServiceOrden();
            IServiceDetalleOrden detalleOrden = new ServiceDetalleOrden();

            try
            {
                var orden = serviceOrden.GetOrdenById(factura.ID_ORDEN);
                if (orden is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "La orden no ha sido encontrada", SweetAlertMessageType.error);
                    return RedirectToAction("DetalleMiOrden");
                }
                var listaDetalle = detalleOrden.GetDetalleOrden().Where(x => x.ID_ORDEN == orden.ID_ORDEN).ToList();

                factura.SUBTOTAL = listaDetalle.Sum(x => x.MONTO_UNIDAD);
                factura.IMPUESTO = factura.SUBTOTAL * 0.13;
                factura.TOTAL = factura.SUBTOTAL + factura.SUBTOTAL;
                factura.FECHA = orden.FECHA;
                factura.ID_MESA = orden.ID_MESA;
                factura.ID_ORDEN = orden.ID_ORDEN;
                serviceFactura.SaveFactura(factura);

                orden.ESTADO_ORDEN = (int)Estado_Orden.Pagada;
                var mesa = serviceMesa.GetMesaById(orden.ID_MESA);
                mesa.ID_ESTADO_MESA = (int)ESTADO_MESA_ENUM.DISPONIBLE;

                serviceOrden.Save(orden);
                serviceMesa.Save(mesa);

                TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Mensaje", "Pago efectuado exitosamente", SweetAlertMessageType.success);
                return RedirectToAction("ListaProductos", "Orden");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Factura";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }
    }
}