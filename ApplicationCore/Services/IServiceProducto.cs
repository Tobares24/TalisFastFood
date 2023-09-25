using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceProducto
    {
        IEnumerable<PRODUCTO> GetProducto();
        PRODUCTO GetProductoyID(int id);
        void DeleteProducto(int id);
        IEnumerable<PRODUCTO> GetProductoPorCategoria(int idCategoria);
        IEnumerable<PRODUCTO> GetProductoByNombre(string nombre);
        IEnumerable<string> GetProductoNombre();
        IEnumerable<PRODUCTO> GetProductoByRestaurant(int restaurantId);
        PRODUCTO Save(PRODUCTO producto, string[] selectedRestaurantes);
    }
}