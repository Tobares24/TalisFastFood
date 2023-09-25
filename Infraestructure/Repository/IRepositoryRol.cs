using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryRol
    {
        IEnumerable<ROL> GetRoles();
        ROL GetRolById(int id);
    }
}
