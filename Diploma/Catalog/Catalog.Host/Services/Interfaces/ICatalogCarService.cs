﻿using Infrastructure.Enums;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Responses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogCarService
{
    Task<PaginatedItemsResponse<CatalogCarDto>?> GetCatalogCarsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);
    Task<CatalogCarDto> GetCatalogCarByIdAsync(int id);
    Task<GroupedEntitiesResponse<CatalogCarDto>> GetCatalogCarByManufacturerAsync(int manufacturerId);
    Task<CatalogCarDto> AddCatalogCarAsync(string model, DateTime year, string transmission, decimal price, string description, string pictureFileName, double engineDisplacement, int quantity, int catalogManufacturerId);
    Task<CatalogCarDto> UpdateCatalogCarAsync(int id, string model, DateTime year, string transmission, decimal price, string description, string pictureFileName, double engineDisplacement, int quantity, int catalogManufacturerId);
    Task<CatalogCarDto> DeleteCatalogCarAsync(int id);
    Task UpdateCatalogCarQuantity(int clientId);
}
