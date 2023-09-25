using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceRol
    {
        IEnumerable<ROL> GetRoles();
        ROL GetRolById(int id);
    }
}
