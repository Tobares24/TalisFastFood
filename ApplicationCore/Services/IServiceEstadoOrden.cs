using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceEstadoOrden
    {
        IEnumerable<ESTADO_ORDEN> GetEstadoOrden();
    }
}
