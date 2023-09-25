using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryDetalleOrden
    {
        IEnumerable<DETALLE_ORDEN> GetDetalleOrden();
        DETALLE_ORDEN GetDetalleById(int id);
    }
}
