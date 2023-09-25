using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryCategoriaProducto
    {
        IEnumerable<CATEGORIA_PRODUCTO> GetCategoria();
        CATEGORIA_PRODUCTO GetCategoriaById(int id);
    }
}