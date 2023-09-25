using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceEstadoMesa : IServiceEstadoMesa
    {
        public IEnumerable<Infraestructure.Models.ESTADO_MESA> GetEstadoMesa()
        {
            IRepositoryEstadoMesa repository = new RepositoryEstadoMesa();
            return repository.GetEstadoMesa();
        }

        public Infraestructure.Models.ESTADO_MESA GetEstadoMesaById(int id)
        {
            IRepositoryEstadoMesa repository = new RepositoryEstadoMesa();
            return repository.GetEstadoMesaById(id);
        }
    }
}
