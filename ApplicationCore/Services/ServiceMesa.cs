using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceMesa : IServiceMesa
    {
        public void DeleteMesa(int id)
        {
            IRepositoryMesa repository = new RepositoryMesa();
            repository.DeleteMesa(id);
        }

        public IEnumerable<MESA> GetMesa()
        {
            IRepositoryMesa repository = new RepositoryMesa();
            return repository.GetMesa();
        }

        public MESA GetMesaById(int id)
        {
            IRepositoryMesa repository = new RepositoryMesa();
            return repository.GetMesaById(id);
        }

        public MESA GetMesaByRestaurante(int idRestaurante)
        {
            IRepositoryMesa repository = new RepositoryMesa();
            return repository.GetMesaByRestaurante(idRestaurante);
        }

        public IEnumerable<MESA> GetMesaPorEsta(int idEstado)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MESA> GetMesasPorRestaurante(int idRestaurante)
        {
            IRepositoryMesa repository = new RepositoryMesa();
            return repository.GetMesasPorRestaurante(idRestaurante);
        }
        public MESA Save(MESA mesa)
        {
            IRepositoryMesa repository = new RepositoryMesa();
            return repository.Save(mesa);
        }
    }
}