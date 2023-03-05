﻿using Order.Host.Models.Dtos;
using Order.Host.Models.Response;

namespace Order.Host.Services.Interfaces
{
    public interface IProductService
    {
        Task<GroupedEntitiesResponse<ProductDto>> GetProductsAsync();
        Task<ProductDto> CreateProductAsync(int id, string model, decimal price);
        Task<ProductDto> UpdateProductAsync(int id, string model, decimal price);
        Task<ProductDto> DeleteProductAsync(int id);
    }
}
