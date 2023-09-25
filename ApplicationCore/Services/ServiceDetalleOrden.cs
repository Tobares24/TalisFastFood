using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceDetalleOrden : IServiceDetalleOrden
    {
        public DETALLE_ORDEN GetOrdenDetalleById(int id)
        {
            IRepositoryDetalleOrden repository = new RepositoryDetalleOrden();
            return repository.GetDetalleById(id);
        }

        public IEnumerable<DETALLE_ORDEN> GetDetalleOrden()
        {
            IRepositoryDetalleOrden repository = new RepositoryDetalleOrden();
            return repository.GetDetalleOrden();
        }
    }
}
