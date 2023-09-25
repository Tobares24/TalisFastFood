using ApplicationCore.Services;
using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Talis_Fast_Food.Utils;
using Web.Security;
using Web.ViewModel;

namespace Talis_Fast_Food.Controllers
{
    public class OrdenController : Controller
    {
        private static RESTAURANTE restaurante;

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("NotificationMessage"))
            {
                ViewBag.NotificationMessage = TempData["NotificationMessage"];
            }

            try
            {
                IServiceOrden serviceOrden = new ServiceOrden();
                var ordenList = serviceOrden.GetOrden();

                IServiceEstadoOrden serviceEstado = new ServiceEstadoOrden();
                ViewBag.listaEstado = serviceEstado.GetEstadoOrden();

                return View(ordenList);
            }

            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }


        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Cliente, (int)Roles.Mesero)]
        public ActionResult DetalleOrden(int? id)
        {
            IServiceOrden _ServiceOrden = new ServiceOrden();
            ORDEN orden = null;
            try
            {
                if (id is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El identificador de la orden es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                orden = _ServiceOrden.GetOrdenById(id.Value);
                if (orden is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "La orden no ha sido encontrada", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Orden";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View(orden);
        }


        [CustomAuthorize((int)Roles.Cliente)]
        public ActionResult IndexCliente()
        {
            return View();
        }

        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Administrador)]
        public ActionResult ListaProductos()
        {
            IEnumerable<PRODUCTO> productoList = null;
            try
            {
                IServiceRestaurante _ServiceRestaurante = new ServiceRestaurante();
                ViewBag.listaRestaurante = _ServiceRestaurante.GetRestaurantes();
                IServiceProducto _ServiceProducto = new ServiceProducto();
                productoList = _ServiceProducto.GetProductoByRestaurant(1);
                return View(productoList);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Administrador)]
        public ActionResult ProductosPorRestaurante(int? id = 1)
        {
            IEnumerable<PRODUCTO> productList = null;
            try
            {
                IServiceRestaurante _ServiceRestaurante = new ServiceRestaurante();
                IServiceProducto _ServiceProducto = new ServiceProducto();
                restaurante = _ServiceRestaurante.GetRestauranteById((int)id);
                ViewBag.listaRestaurante = _ServiceRestaurante.GetRestaurantes();
                productList = _ServiceProducto.GetProductoByRestaurant((int)id).ToList();
                return PartialView("_PartialViewListaProductos", productList);
            }

            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        private List<MESA> MesasByRestaurant(int? id)
        {
            List<MESA> listaMesas = null;
            using (var ctx = new MyContext())
            {
                listaMesas = ctx.MESA.Include("RESTAURANTE").Where(x => x.ID_RESTAURANTE == id).ToList();
                return listaMesas;
            }
        }

        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Mesero, (int)Roles.Administrador)]
        public ActionResult IndexOrden()
        {
            double total = Carrito.Instance.CalcularImpuesto() + Carrito.Instance.GetTotal();
            if (TempData.ContainsKey("NotificationMessage"))
            {
                ViewBag.NotificationMessage = TempData["NotificationMessage"];
            }
            ViewBag.DetalleOrden = Carrito.Instance.Items;
            ViewBag.Total = total;
            ViewBag.Clientes = ListaClientes();
            ViewBag.Impuesto = Carrito.Instance.CalcularImpuesto();
            ViewBag.DetalleOrdenMesero = CarritoMesero.Instance.Items;
            ViewBag.TotalOrdenMesero = CarritoMesero.Instance.GetTotal();
            return View();
        }

        [CustomAuthorize((int)Roles.Mesero)]
        public ActionResult ListaOrdenes()
        {
            IEnumerable<ORDEN> ordenList = null;
            try
            {
                IServiceOrden _ServiceOrden = new ServiceOrden();
                ordenList = _ServiceOrden.GetOrden();

                IServiceEstadoOrden _ServiceEstado = new ServiceEstadoOrden();
                ViewBag.listaEstado = _ServiceEstado.GetEstadoOrden();

                return View(ordenList);
            }

            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Mesero, (int)Roles.Administrador)]
        public ActionResult OrdenarProducto(int? idProducto)
        {
            int cantidadProductos;
            var oUsuario = (USUARIO)Session["User"];
            if (oUsuario.ID_ROL == (int)Roles.Cliente)
            {
                cantidadProductos = Carrito.Instance.GetCountItems();
                ViewBag.NotificationCarrito = Carrito.Instance.AgregarItem((int)idProducto);
                return PartialView("_OrdenCantidad");
            }
            else if (oUsuario.ID_ROL == (int)Roles.Mesero)
            {
                cantidadProductos = CarritoMesero.Instance.GetCountItems();
                ViewBag.NotificationCarrito = CarritoMesero.Instance.AgregarItem((int)idProducto);
                return PartialView("_OrdenCantidadMesero");
            }
            return View("ListaProductos");
        }

        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Mesero, (int)Roles.Administrador)]
        public ActionResult ActualizarCantidad(int idProducto, int cantidad)
        {
            var oUsuario = (USUARIO)Session["User"];
            if (oUsuario.ID_ROL == (int)Roles.Cliente)
            {
                ViewBag.DetalleOrden = Carrito.Instance.Items;
                TempData["NotiCarrito"] = Carrito.Instance.SetItemCantidad(idProducto, cantidad);
                TempData.Keep();
                return PartialView("_DetalleOrden", Carrito.Instance.Items);
            }
            else if (oUsuario.ID_ROL == (int)Roles.Mesero)
            {
                ViewBag.DetalleOrden = Carrito.Instance.Items;
                TempData["NotiCarrito"] = CarritoMesero.Instance.SetItemCantidad(idProducto, cantidad);
                TempData.Keep();
                return PartialView("_DetalleOrdenMesero", CarritoMesero.Instance.Items);
            }
            return View("ListaProductos");
        }

        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Mesero)]
        public ActionResult MisOrdenes()
        {
            var usuarioLogueado = (USUARIO)Session["User"];
            IServiceOrden serviceOrden = new ServiceOrden();
            List<ORDEN> lista = null;
            try
            {
                lista = serviceOrden.GetOrdenByUser(usuarioLogueado.ID_USUARIO).ToList();
                if (lista.Count <= 0)
                {
                    ViewBag.listaVacia = "No posee órdenes";
                }
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Orden";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Mesero)]
        public ActionResult OrdenarProductoWaiter(int? idProducto, int? idMesa)
        {
            int cantidadProductos = CarritoMesero.Instance.GetCountItems();
            ViewBag.NotificationCarrito = CarritoMesero.Instance.AgregarItemByWaiter((int)idProducto, (int)idMesa);
            return PartialView("_OrdenCantidadMesero");
        }

        [CustomAuthorize((int)Roles.Mesero)]
        public ActionResult OrdenByWaiter(int idMesa)
        {
            return View();
        }

        [CustomAuthorize((int)Roles.Mesero, (int)Roles.Administrador)]
        public ActionResult ListProductsByRestaurants()
        {
            IEnumerable<PRODUCTO> productList = null;
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                var oUsuario = (USUARIO)Session["User"];
                productList = serviceProducto.GetProductoByRestaurant(oUsuario.ID_RESTAURANTE).ToList();
                return View(productList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [CustomAuthorize((int)Roles.Mesero)]
        public ActionResult DetalleOrdenM(int? id)
        {
            return DetalleOrden(id);
        }

        [CustomAuthorize((int)Roles.Cliente)]
        public ActionResult DetalleMiOrden(int? id)
        {
            if (TempData.ContainsKey("NotificationMessage"))
            {
                ViewBag.NotificationMessage = TempData["NotificationMessage"];
            }

            IServiceOrden _ServiceOrden = new ServiceOrden();
            ORDEN orden = null;
            try
            {
                if (id is null)
                {
                    return RedirectToAction("Index");
                }

                orden = _ServiceOrden.GetOrdenById(id.Value);
                if (orden is null)
                {
                    TempData["NotificationMessage"] = "No existe la orden solicitado";
                    TempData["Redirect"] = "Orden";
                    TempData["Redirect-Action"] = "Index";
                    return RedirectToAction("Default", "Error");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Orden";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
            return View(orden);
        }

        private SelectList ListaClientes()
        {
            try
            {
                IServiceUsuario serviceCliente = new ServiceUsuario();
                IEnumerable<USUARIO> listaClientes = serviceCliente.GetUsuarios().Where(x => x.ID_ROL == (int)Roles.Cliente);
                return new SelectList(listaClientes, "ID_USUARIO", "ID_USUARIO");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private SelectList listaEstados(int idEstado = 0)
        {
            try
            {
                IServiceEstadoOrden serviceEstado = new ServiceEstadoOrden();
                IEnumerable<ESTADO_ORDEN> listaEstados = serviceEstado.GetEstadoOrden();
                return new SelectList(listaEstados, "ID_ESTADO_ORDEN", "ESTADO", idEstado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Mesero)]
        public PartialViewResult ordenesxEstado(int? id)
        {
            IEnumerable<ORDEN> lista = null;
            IServiceOrden serviceOrden = new ServiceOrden();

            try
            {
                if (id != null)
                {
                    lista = serviceOrden.GetOrdenPorEstado((int)id);
                }
                return PartialView("_PartialViewOrdenAdmin", lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [CustomAuthorize((int)Roles.Cliente, (int)Roles.Mesero, (int)Roles.Administrador)]
        public ActionResult Save(ORDEN pOrden)
        {
            int consecutivo = ObtenerConsecutivo() + 1;
            ORDEN orden = new ORDEN();
            MESA mesa = new MESA();
            IServiceMesa serviceMesa = new ServiceMesa();
            USUARIO oUsuario = (USUARIO)Session["User"];
            oUsuario.ID_RESTAURANTE = (restaurante is null) ? 1 : restaurante.ID_RESTAURANTE;

            if (Carrito.Instance.Items.Count() == 0)
            {
                TempData["NotificationMessage"] = Utils.SweetAlertHelper.Mensaje("Orden", "Seleccione los productos a ordenar", SweetAlertMessageType.warning);
                return RedirectToAction("ListaProductos");
            }

            if (oUsuario.ID_ROL == (int)Roles.Cliente)
            {
                if (Carrito.Instance.Items.Count() > 0)
                {
                    orden.ID_USUARIO = oUsuario.ID_USUARIO;
                    orden.ESTADO_ORDEN = (int)Estado_Orden.Pendiente;
                    mesa = serviceMesa.GetMesaByRestaurante(oUsuario.ID_RESTAURANTE);
                    mesa.ID_ESTADO_MESA = (int)ESTADO_MESA_ENUM.ORDEN_REGISTRADA;
                    orden.ID_MESA =  mesa.ID_MESA;
                    orden.FECHA = pOrden.FECHA;
                    var listaDetalle = Carrito.Instance.Items;

                    foreach (var item in listaDetalle)
                    {
                        DETALLE_ORDEN detalleOrden = new DETALLE_ORDEN()
                        {
                            ID_ORDEN = consecutivo,
                            ID_PRODUCTO = item.ID_PRODUCTO,
                            CANTIDAD = item.CANTIDAD,
                            MONTO_UNIDAD = item.MONTO_UNIDAD
                        };
                        orden.DETALLE_ORDEN.Add(detalleOrden);
                    }
                }
            }
            else if (oUsuario.ID_ROL == (int)Roles.Mesero || oUsuario.ID_ROL == (int)Roles.Administrador)
            {
                if (CarritoMesero.Instance.Items.Count() > 0)
                {
                    orden.ID_USUARIO = oUsuario.ID_USUARIO;
                    orden.ESTADO_ORDEN = (int)Estado_Orden.Pendiente;
                    mesa = serviceMesa.GetMesaByRestaurante(oUsuario.ID_RESTAURANTE);
                    mesa.ID_ESTADO_MESA = (int)ESTADO_MESA_ENUM.ORDEN_REGISTRADA;
                    orden.ID_MESA = mesa.ID_MESA;
                    orden.FECHA = pOrden.FECHA;
                    var listaDetalle = CarritoMesero.Instance.Items;

                    foreach (var item in listaDetalle)
                    {
                        DETALLE_ORDEN detalleOrden = new DETALLE_ORDEN()
                        {
                            ID_ORDEN = consecutivo,
                            ID_PRODUCTO = item.ID_PRODUCTO,
                            CANTIDAD = item.CANTIDAD,
                            MONTO_UNIDAD = item.MONTO_UNIDAD
                        };
                        orden.DETALLE_ORDEN.Add(detalleOrden);
                    }
                }
            }
            else
            {
                TempData["NotificationMessage"] = Utils.SweetAlertHelper.Mensaje("Orden", "Seleccione los producto a ordenar", SweetAlertMessageType.warning);
                return RedirectToAction("ListaProductos");
            }

            IServiceOrden serviceOrden = new ServiceOrden();
            serviceOrden.Save(orden);
            serviceMesa.Save(mesa);
            Carrito.Instance.EliminarCarrito();
            CarritoMesero.Instance.EliminarCarrito();

            if (oUsuario.ID_ROL == (int)Roles.Cliente)
            {
                TempData["NotificationMessage"] = Utils.SweetAlertHelper.Mensaje("Orden", "La orden ha sido registrada exitosamente", SweetAlertMessageType.success);
                return RedirectToAction("MisOrdenes");
            }
            else
            {
                TempData["NotificationMessage"] = Utils.SweetAlertHelper.Mensaje("Orden", "La orden ha sido registrada exitosamente", SweetAlertMessageType.success);
                return RedirectToAction("ListaOrdenes");
            }
        }

        private int ObtenerConsecutivo()
        {
            IRepositoryOrden _Repository = new RepositoryOrden();

            var lista = _Repository.GetOrden();

            int consecutivo = 0;

            foreach (var item in lista)
            {
                consecutivo = item.ID_ORDEN;
            }

            return consecutivo;
        }
    }
}