using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryRestaurante
    {
        IEnumerable<RESTAURANTE> GetRestaurantes();
        RESTAURANTE GetRestauranteById(int idRestaurante);
    }
}
