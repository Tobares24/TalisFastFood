using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceFactura : IServiceFactura
    {
        public FACTURA GetFacturaById(int id)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaById(id);
        }

        public IEnumerable<FACTURA> GetFacturas()
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturas();
        }

        public IEnumerable<FACTURA> GetFacturaUsuario(string id)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaUsuario(id);
        }

        public FACTURA SaveFactura(FACTURA factura)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.SaveFactura(factura);
        }
    }
}