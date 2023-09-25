using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Talis_Fast_Food.Utils;
using Web.Security;
using Web.Utils;

namespace Talis_Fast_Food.Controllers
{
    public class ProductosController : Controller
    {
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("NotificationMessage"))
            {
                ViewBag.NotificationMessage = TempData["NotificationMessage"];
            }

            try
            {
                IServiceProducto serviceProducto = new ServiceProducto();
                var productoList = serviceProducto.GetProducto();
                IServiceCategoriaProducto _ServiceCategoria = new ServiceCategoria();
                ViewBag.listaCategorias = _ServiceCategoria.GetCategoria();
                ViewBag.listaNombres = serviceProducto.GetProductoNombre();

                return View(productoList);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        public PartialViewResult productosxCategoria(int? id)
        {
            IEnumerable<PRODUCTO> lista = null;
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                if (id is null)
                {
                    lista = serviceProducto.GetProductoPorCategoria((int)id);
                }
                return PartialView("_PartialViewProductoAdmin", lista);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult DetalleProductos(int? id)
        {
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                if (id is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El identificador del existeProducto es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var existeProducto = serviceProducto.GetProductoyID(Convert.ToInt32(id));

                if (existeProducto is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El producto no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
                return View(existeProducto);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Eliminar(int? id)
        {
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                if (id is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El identificador del producto es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var existeProducto = serviceProducto.GetProductoyID((int)id);
                if (existeProducto is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El producto no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
                return View(existeProducto);
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult EliminarProducto(int? id)
        {
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                if (id is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El identificador del producto es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var existeProducto = serviceProducto.GetProductoyID((int)id);
                if (existeProducto is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El producto no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }
                serviceProducto.DeleteProducto((int)id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Utils.Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult buscarProductoxNombre(string filtro)
        {
            IEnumerable<PRODUCTO> lista = null;
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    lista = serviceProducto.GetProducto();
                }
                else
                {
                    lista = serviceProducto.GetProductoByNombre(filtro);
                }
                return PartialView("_PartialViewProductoAdmin", lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Mensaje()
        {
            return View();
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult AgregarProductos()
        {
            try
            {
                ViewBag.IdRestaurante = listaRestaurantes();
                ViewBag.IdCategoria = listaCategorias();
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Save(PRODUCTO producto, string[] selectedRestaurantes)
        {
            IServiceProducto _ServiceProducto = new ServiceProducto();
            try
            {
                if (ModelState.IsValid)
                {
                    var oProducto = _ServiceProducto.Save(producto, selectedRestaurantes);
                    ViewBag.NotificationMessage = Utils.SweetAlertHelper.Mensaje("Mensaje",
                            "Producto guardado exitosamente", Utils.SweetAlertMessageType.success);

                    return RedirectToAction("Mensaje");
                }
                else
                {
                    Util.ValidateErrors(this);
                    ViewBag.IdRestaurante = listaRestaurantes(producto.RESTAURANTE);
                    ViewBag.IdCategoria = listaCategorias(producto.ID_CATEGORIA);

                    ViewBag.NotificationMessage = Utils.SweetAlertHelper.Mensaje("Producto",
                            "Producto no válido", Utils.SweetAlertMessageType.warning);
                    return View("AgregarProductos", producto);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        private SelectList listaCategorias(int id = 0)
        {
            IServiceCategoriaProducto serviceProducto = new ServiceCategoria();
            try
            {
                IEnumerable<CATEGORIA_PRODUCTO> listaCategorias = serviceProducto.GetCategoria();
                return new SelectList(listaCategorias, "ID_CATEGORIA", "NOMBRE");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MultiSelectList listaRestaurantes(ICollection<RESTAURANTE> restaurantes = null)
        {
            IServiceRestaurante serviceRestaurante = new ServiceRestaurante();
            try
            {
                IEnumerable<RESTAURANTE> listaRestaurantes = serviceRestaurante.GetRestaurantes();
                int[] listaRestaurantesSelect = null;
                if (restaurantes != null)
                {
                    listaRestaurantesSelect = restaurantes.Select(c => c.ID_RESTAURANTE).ToArray();
                }

                return new MultiSelectList(listaRestaurantes, "ID_RESTAURANTE", "NOMBRE", listaRestaurantesSelect);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult EditarProductos(int? id)
        {
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                if (id is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El identificador del producto es requerido", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                var existeProducto = serviceProducto.GetProductoyID((int)id);
                if (existeProducto is null)
                {
                    TempData["NotificationMessage"] = SweetAlertHelper.Mensaje("Error", "El producto no ha sido encontrado", Utils.SweetAlertMessageType.error);
                    return RedirectToAction("Index");
                }

                ViewBag.IdRestaurante = listaRestaurantes(existeProducto.RESTAURANTE);
                ViewBag.IdCategoria = listaCategorias(existeProducto.ID_CATEGORIA);

                return View(existeProducto);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["NotificationMessage"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }
    }
}