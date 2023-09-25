using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryMesa
    {
        IEnumerable<MESA> GetMesa();
        IEnumerable<MESA> GetMesasPorRestaurante(int idRestaurante);
        MESA GetMesaById(int id);
        MESA GetMesaByRestaurante(int idRestaurante);
        void DeleteMesa(int id);
        MESA Save(MESA mesa);
    }
}