using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryEstadoMesa
    {
        IEnumerable<Models.ESTADO_MESA> GetEstadoMesa();
        Models.ESTADO_MESA GetEstadoMesaById(int id);
    }
}
