using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{

    public class ServiceProducto : IServiceProducto
    {
        public IEnumerable<PRODUCTO> GetProducto()
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProducto();
        }

        public IEnumerable<PRODUCTO> GetProductoPorCategoria(int idCategoria)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoPorCategoria(idCategoria);
        }

        public IEnumerable<string> GetProductoNombre()
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProducto().Select(x => x.NOMBRE);
        }

        public IEnumerable<PRODUCTO> GetProductoByNombre(string nombre)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByNombre(nombre);
        }

        public IEnumerable<PRODUCTO> GetProductoByRestaurant(int restaurantId)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByRestaurant(restaurantId);
        }

        public PRODUCTO GetProductoyID(int id)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByID(id);
        }

        public PRODUCTO Save(PRODUCTO producto, string[] selectedRestaurantes)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.Save(producto, selectedRestaurantes);
        }

        public void DeleteProducto(int id)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            repository.DeleteProducto(id);
        }
    }
}