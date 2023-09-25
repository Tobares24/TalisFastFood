using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceCategoriaProducto
    {
        IEnumerable<CATEGORIA_PRODUCTO> GetCategoria();
        CATEGORIA_PRODUCTO GetCategoriaById(int id);
    }
}