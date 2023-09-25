using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceMesa
    {
        IEnumerable<MESA> GetMesa();
        IEnumerable<MESA> GetMesaPorEsta(int idEstado);
        IEnumerable<MESA> GetMesasPorRestaurante(int idRestaurante);
        MESA GetMesaById(int id);
        MESA GetMesaByRestaurante(int idRestaurante);
        void DeleteMesa(int id);
        MESA Save(MESA mesa);
    }
}