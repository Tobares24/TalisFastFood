using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceOrden
    {
        IEnumerable<ORDEN> GetOrden();
        ORDEN GetOrdenById(int id);
        IEnumerable<ORDEN> GetOrdenPorEstado(int idEstado);
        IEnumerable<ORDEN> GetOrdenByUser(string idUsuario);
        void DeleteOrden(int id);
        ORDEN Save(ORDEN orden);
        ORDEN GetOrdenByFactura(int idFactura);
    }
}
