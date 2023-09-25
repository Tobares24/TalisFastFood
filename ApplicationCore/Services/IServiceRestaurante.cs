using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceRestaurante
    {
        IEnumerable<RESTAURANTE> GetRestaurantes();
        RESTAURANTE GetRestauranteById(int idRestaurante);
    }
}
