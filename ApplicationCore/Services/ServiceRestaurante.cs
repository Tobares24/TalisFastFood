using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceRestaurante : IServiceRestaurante
    {
        public RESTAURANTE GetRestauranteById(int idRestaurante)
        {
            IRepositoryRestaurante repository = new RepositoryRestaurante();
            return repository.GetRestauranteById(idRestaurante);
        }

        public IEnumerable<RESTAURANTE> GetRestaurantes()
        {
            IRepositoryRestaurante repository = new RepositoryRestaurante();
            return repository.GetRestaurantes();
        }
    }
}
