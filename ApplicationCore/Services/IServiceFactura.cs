using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceFactura
    {
        IEnumerable<FACTURA> GetFacturas();
        FACTURA GetFacturaById(int id);
        FACTURA SaveFactura(FACTURA factura);
        IEnumerable<FACTURA> GetFacturaUsuario(string id);
    }
}