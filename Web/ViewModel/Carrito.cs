using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talis_Fast_Food.Utils;

namespace Web.ViewModel
{
    public class Carrito
    {
        public List<ViewModelOrdenDetalle> Items { get; private set; }
        public IDictionary<int, List<ViewModelOrdenDetalle>> DataOrden { get; private set; }

        public static Carrito Instance;

        static Carrito()
        {
            if (HttpContext.Current.Session["Carrito"] is null)
            {
                Instance = new Carrito();
                Instance.Items = new List<ViewModelOrdenDetalle>();
                Instance.DataOrden = new Dictionary<int, List<ViewModelOrdenDetalle>>();
                HttpContext.Current.Session["Carrito"] = Instance;
            }
            else
            {
                Instance = (Carrito)HttpContext.Current.Session["Carrito"];
            }
        }

        public string AgregarItemByWaiter(int idProducto, int idMesa)
        {
            string mensaje = "";
            ViewModelOrdenDetalle newItem = new ViewModelOrdenDetalle(idProducto);

            if (newItem != null)
            {
                if (Items.Exists(x => x.ID_PRODUCTO == idProducto))
                {
                    ViewModelOrdenDetalle item = Items.Find(x => x.ID_PRODUCTO == idProducto);
                    item.CANTIDAD++;
                    if (DataOrden.ContainsKey(idMesa))
                    {

                        DataOrden = new Dictionary<int, List<ViewModelOrdenDetalle>>();
                        DataOrden[idMesa] = Items;
                    }
                }
                else
                {
                    newItem.CANTIDAD = 1;
                    Items.Add(newItem);
                    DataOrden = new Dictionary<int, List<ViewModelOrdenDetalle>>();
                    DataOrden.Add(idMesa, Items);
                }
                mensaje = SweetAlertHelper.Mensaje("Orden Producto", "Producto agregado a la orden", SweetAlertMessageType.success);
            }
            else
            {
                mensaje = SweetAlertHelper.Mensaje("Orden Producto", "El producto solicitado no existe", SweetAlertMessageType.error);
            }
            return mensaje;
        }

        public string AgregarItem(int idProducto)
        {
            string mensaje = "";
            ViewModelOrdenDetalle newItem = new ViewModelOrdenDetalle(idProducto);

            if (newItem != null)
            {
                if (Items.Exists(x => x.ID_PRODUCTO == idProducto))
                {
                    ViewModelOrdenDetalle item = Items.Find(x => x.ID_PRODUCTO == idProducto);
                    item.CANTIDAD++;
                }
                else
                {
                    newItem.CANTIDAD = 1;
                    Items.Add(newItem);
                }
                mensaje = SweetAlertHelper.Mensaje("Orden Producto", "Producto agregado a la orden", SweetAlertMessageType.success);
            }
            else
            {
                mensaje = SweetAlertHelper.Mensaje("Orden Producto", "El producto solicitado no existe", SweetAlertMessageType.error);
            }
            return mensaje;
        }

        public string SetItemCantidad(int idProducto, int cantidad)
        {
            string mensaje = "";

            if (cantidad == 0)
            {
                EliminarItem(idProducto);
                mensaje = SweetAlertHelper.Mensaje("Orden Producto", "Producto eliminado", SweetAlertMessageType.success);
            }
            else
            {
                ViewModelOrdenDetalle updateItem = new ViewModelOrdenDetalle(idProducto);
                if (Items.Exists(x => x.ID_PRODUCTO == idProducto))
                {
                    ViewModelOrdenDetalle item = Items.Find(x => x.ID_PRODUCTO == idProducto);
                    item.CANTIDAD = cantidad;
                    mensaje = SweetAlertHelper.Mensaje("Orden Producto", "Cantidad actualizada", SweetAlertMessageType.success);
                }
            }
            return mensaje;
        }

        public string EliminarItem(int idProducto)
        {
            string mensaje = "El libro no existe";
            if (Items.Exists(x => x.ID_PRODUCTO == idProducto))
            {
                var itemEliminar = Items.Single(x => x.ID_PRODUCTO == idProducto);
                Items.Remove(itemEliminar);
                mensaje = SweetAlertHelper.Mensaje("Orden Producto", "Producto eliminado", SweetAlertMessageType.success);
            }
            return mensaje;

        }

        public double GetTotal()
        {
            double total = 0;
            total = Items.Sum(x => x.SubTotal);

            return total;
        }

        public double CalcularImpuesto()
        {
            double total = Items.Sum(x => x.SubTotal);
            double impuesto = total * 0.13;
            return impuesto;
        }

        public int GetCountItems()
        {
            int total = 0;
            total = Items.Sum(x => x.CANTIDAD);

            return total;
        }

        public void EliminarCarrito()
        {
            Items.Clear();
            HttpContext.Current.Session.Remove("Carrito");
        }
    }
}