using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceOrden : IServiceOrden
    {
        public void DeleteOrden(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ORDEN> GetOrdenPorEstado(int idEstado)
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.GetOrdenPorEstado(idEstado);
        }

        public IEnumerable<ORDEN> GetOrden()
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.GetOrden();
        }

        public ORDEN GetOrdenById(int id)
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.GetOrdenById(id);
        }

        public ORDEN GetOrdenByFactura(int idFactura)
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.GetOrdenByFactura(idFactura);
        }

        public IEnumerable<ORDEN> GetOrdenByUser(string idUsuario)
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.GetOrdenByUser(idUsuario);
        }

        public ORDEN Save(ORDEN orden)
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.Save(orden);
        }
    }
}
