using ApplicationCore.Services;
using Infraestructure.Models;

namespace Web.ViewModel
{
    public class ViewModelOrdenDetalle
    {
        public int ID_MESA { get; set; }
        public int ID_PRODUCTO { get; set; }
        public int CANTIDAD { get; set; }
        public double MONTO_UNIDAD
        {
            get { return PRODUCTO.PRECIO; }
        }
        public string NOTA { get; set; }
        public virtual PRODUCTO PRODUCTO { get; set; }

        public double SubTotal
        {
            get
            {
                return CalculoSubTotal();
            }
        }

        private double CalculoSubTotal()
        {
            return this.MONTO_UNIDAD * this.CANTIDAD;
        }

        public ViewModelOrdenDetalle(int idProducto)
        {
            IServiceProducto serviceProducto = new ServiceProducto();
            IServiceMesa serviceMesa = new ServiceMesa();
            this.PRODUCTO = serviceProducto.GetProductoyID(idProducto);
            this.ID_PRODUCTO = idProducto;
        }
    }
}