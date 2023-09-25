using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceDetalleOrden
    {
        IEnumerable<DETALLE_ORDEN> GetDetalleOrden();
        DETALLE_ORDEN GetOrdenDetalleById(int id);
    }
}
