using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceCategoria : IServiceCategoriaProducto
    {
        public IEnumerable<CATEGORIA_PRODUCTO> GetCategoria()
        {
            IRepositoryCategoriaProducto repository = new RepositoryCategoria();
            return repository.GetCategoria();
        }

        public CATEGORIA_PRODUCTO GetCategoriaById(int id)
        {
            RepositoryCategoria repository = new RepositoryCategoria();
            return repository.GetCategoriaById(id);
        }
    }
}