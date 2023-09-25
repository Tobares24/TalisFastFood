using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryFactura
    {
        IEnumerable<FACTURA> GetFacturas();
        FACTURA GetFacturaById(int id);
        FACTURA SaveFactura(FACTURA factura);
        IEnumerable<FACTURA> GetFacturaUsuario(string idUsuario);
    }
}