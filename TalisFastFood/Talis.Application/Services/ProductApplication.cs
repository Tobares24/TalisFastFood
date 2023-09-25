using AutoMapper;
using Talis.Application.Commons;
using Talis.Application.Dtos;
using Talis.Application.Interfaces;
using Talis.Infrastructure.Commons.Bases.Request;
using Talis.Infrastructure.Commons.Bases.Response;
using Talis.Infrastructure.Persistences.Interfaces;
using Talis.Utilities.Static;

namespace Talis.Application.Services
{
    public class ProductApplication : IProductApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<BaseResponse<BaseEntityResponse<ProductRequestDto>>> GetAllAsyncProduct(BaseFiltersRequest filters)

        {
            try
            {
                var response = new BaseResponse<BaseEntityResponse<ProductRequestDto>>();
                var products = await _unitOfWork.Product.GetAllAsync(filters);

                if (products is not null)
                {
                    response.Data = _mapper.Map<BaseEntityResponse<ProductRequestDto>>(products);
                    Console.WriteLine(ReplyMessage.MESSAGE_QUERY);
                }
                else
                {
                    Console.WriteLine(ReplyMessage.MESSAGE_QUERY_EMPTY);
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
