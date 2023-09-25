using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceEstadoMesa
    {
        IEnumerable<Infraestructure.Models.ESTADO_MESA> GetEstadoMesa();
        Infraestructure.Models.ESTADO_MESA GetEstadoMesaById(int id);
    }
}
