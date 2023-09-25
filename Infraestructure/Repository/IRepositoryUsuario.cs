using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryUsuario
    {
        IEnumerable<USUARIO> GetUsuarios();
        USUARIO GetUsuarioById(string id);
        IEnumerable<USUARIO> GetMeseros();
        IEnumerable<USUARIO> GetUsuariosById(string id);
        USUARIO Save(USUARIO usuario);
        USUARIO GetUsuario(string email, string password);
        void DeleteUsuario(string id);
    }
}
;