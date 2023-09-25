using Infraestructure.Models;
using Infraestructure.Repository;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public class ServiceRol : IServiceRol
    {
        public ROL GetRolById(int id)
        {
            IRepositoryRol repository = new RepositoryRol();
            return repository.GetRolById(id);
        }

        public IEnumerable<ROL> GetRoles()
        {
            IRepositoryRol repository = new RepositoryRol();
            return repository.GetRoles();
        }
    }
}
