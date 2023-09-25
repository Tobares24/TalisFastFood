using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceUsuario
    {
        IEnumerable<USUARIO> GetUsuarios();
        USUARIO GetUsuarioById(string id);
        IEnumerable<USUARIO> GetUsuarioByCedula(string id);
        IEnumerable<USUARIO> GetMeseros();
        IEnumerable<USUARIO> GetUsuariosById(string id);
        void DeleteUsuario(string id);
        USUARIO Save(USUARIO usuario);
        USUARIO GetUsuario(string email, string password);
    }
}
