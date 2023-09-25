using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talis.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork 
    {
        IProductRepository Product { get; }
        void Commit();
        void RollBack();
    }
}
