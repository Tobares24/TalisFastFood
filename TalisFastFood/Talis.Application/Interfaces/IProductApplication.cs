using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talis.Application.Commons;
using Talis.Application.Dtos;
using Talis.Infrastructure.Commons.Bases.Request;
using Talis.Infrastructure.Commons.Bases.Response;

namespace Talis.Application.Interfaces
{
    public interface IProductApplication
    {
        Task<BaseResponse<BaseEntityResponse<ProductRequestDto>>> GetAllAsyncProduct(BaseFiltersRequest filters);
    }
}
