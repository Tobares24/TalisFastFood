using Infraestructure.Models;
using System.Collections.Generic;

namespace Web.ViewModel
{
    public class ViewModelOrden
    {
        public MESA Mesa { get; set; }
        public Infraestructure.Models.ESTADO_MESA Estado_Mesa { get; set; }
        public List<Carrito> DetalleOrden { get; set; }

        public ViewModelOrden()
        {
            DetalleOrden = null;
        }
    }
}