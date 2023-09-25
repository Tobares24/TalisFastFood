using Talis.Domain.Models;
using Talis.Infrastructure.Commons.Bases.Request;
using Talis.Infrastructure.Commons.Bases.Response;

namespace Talis.Infrastructure.Persistences.Interfaces
{
    public interface IProductRepository
    {
        Task<BaseEntityResponse<Product>> GetAllAsync(BaseFiltersRequest filters);
    }
}
