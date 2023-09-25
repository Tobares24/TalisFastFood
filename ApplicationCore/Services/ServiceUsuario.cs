using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        public void DeleteUsuario(string id)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            repository.DeleteUsuario(id);
        }

        public IEnumerable<USUARIO> GetMeseros()
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.GetMeseros();
        }

        public IEnumerable<USUARIO> GetUsuarios()
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.GetUsuarios();
        }

        public USUARIO GetUsuario(string email, string password)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.GetUsuario(email, password);
        }

        public USUARIO GetUsuarioById(string id)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.GetUsuarioById(id);
        }

        public IEnumerable<USUARIO> GetUsuarioByCedula(string id)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.GetUsuariosById(id);
        }

        public USUARIO Save(USUARIO usuario)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            return repository.Save(usuario);
        }

        public IEnumerable<USUARIO> GetUsuariosById(string id)
        {
            throw new NotImplementedException();
        }
    }
}