using Infraestructure.Models;
using System;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryProducto
    {
        IEnumerable<PRODUCTO> GetProducto();
        PRODUCTO GetProductoByID(int id);
        void DeleteProducto(int id);
        IEnumerable<PRODUCTO> GetProductoByNombre(String nombre);
        IEnumerable<PRODUCTO> GetProductoPorCategoria(int idCategoria);
        IEnumerable<PRODUCTO> GetProductoByRestaurant(int restaurantId);
        PRODUCTO Save(PRODUCTO producto, string[] selectedRestaurantes);
    }
}