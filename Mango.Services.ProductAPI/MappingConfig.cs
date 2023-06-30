using AutoMapper;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        //method to return mapping configuration for automapper
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product >();
                config.CreateMap<Product, ProductDto>();
            }
            );

            return mappingConfig;
        }
    }
}
