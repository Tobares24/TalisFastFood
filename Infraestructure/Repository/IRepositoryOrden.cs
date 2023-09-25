using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryOrden
    {
        IEnumerable<ORDEN> GetOrden();
        ORDEN GetOrdenByFactura(int idFactura);
        ORDEN GetOrdenById(int id);
        IEnumerable<ORDEN> GetOrdenByUser(string idUsuario);
        IEnumerable<ORDEN> GetOrdenPorEstado(int idEstado);
        ORDEN Save(ORDEN usuario);
    }
}
