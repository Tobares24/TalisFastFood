using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talis.Application.Dtos;
using Talis.Domain.Models;
using Talis.Infrastructure.Commons.Bases.Response;

namespace Talis.Application.Mappers
{
    public class CategoryMappingsProfile : Profile
    {
        public CategoryMappingsProfile()
        {
            CreateMap<ProductRequestDto, Product>()
                .ReverseMap();
            CreateMap<BaseEntityResponse<Product>, BaseEntityResponse<ProductRequestDto>>()
                        .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                        .ReverseMap();
        }
    }
}
