using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceEstadoOrden : IServiceEstadoOrden
    {
        public IEnumerable<ESTADO_ORDEN> GetEstadoOrden()
        {
            IRepositoryEstadoOrden repository = new RepositoryEstadoOrden();
            return repository.GetEstadoOrden();
        }
    }
}
