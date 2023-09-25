using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryEstadoOrden
    {
        IEnumerable<ESTADO_ORDEN> GetEstadoOrden();
        ESTADO_ORDEN GetEstadoById(int id);
    }
}
