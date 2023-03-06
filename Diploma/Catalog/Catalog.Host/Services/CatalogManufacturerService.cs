﻿using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Responses;

namespace Catalog.Host.Services;

public class CatalogManufacturerService : BaseDataService<ApplicationDbContext>, ICatalogManufacturerService
{
    private readonly ICatalogManufacturerRepository _catalogManufacturerRepository;
    private readonly ILogger<BaseDataService<ApplicationDbContext>> _logger;
    private readonly IMapper _mapper;

    public CatalogManufacturerService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogManufacturerRepository catalogManufacturerRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _logger = logger;
        _catalogManufacturerRepository = catalogManufacturerRepository;
        _mapper = mapper;
    }

    public async Task<CatalogManufacturerDto> CreateCatalogManufacturerAsync(string name, DateTime foundationYear, string headquartersLocation)
    {
        _logger.LogInformation($"CreateCatalogManufacturerAsync method with the following parameters: name = {name}, foundationYear = {foundationYear}, headquartersLocation = {headquartersLocation}");
        return await ExecuteSafeAsync(async () =>
        {
            return _mapper.Map<CatalogManufacturerDto>(await _catalogManufacturerRepository.AddCatalogManufacturerAsync(name, foundationYear, headquartersLocation));
        });
    }

    public async Task<CatalogManufacturerDto> DeleteCatalogManufacturerAsync(int id)
    {
        _logger.LogInformation($"DeleteCatalogManufacturerAsync method with the following parameters: id = {id}");
        return await ExecuteSafeAsync(async () =>
        {
            return _mapper.Map<CatalogManufacturerDto>(await _catalogManufacturerRepository.DeleteCatalogManufacturerAsync(id));
        });
    }

    public async Task<GroupedEntitiesResponse<CatalogManufacturerDto>> GetCatalogManufacturersAsync()
    {
        _logger.LogInformation($"GetCatalogManufacturersAsync executed");
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogManufacturerRepository.GetCatalogManufacturersAsync();
            return new GroupedEntitiesResponse<CatalogManufacturerDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogManufacturerDto>(s)).ToList()
            };
        });
    }

    public async Task<CatalogManufacturerDto> UpdateCatalogManufacturerAsync(int id, string name, DateTime foundationYear, string headquartersLocation)
    {
        _logger.LogInformation($"UpdateCatalogManufacturerAsync method with the following parameters: id = {id}, name = {name}, foundationYear = {foundationYear}, headquartersLocation = {headquartersLocation}");
        return await ExecuteSafeAsync(async () =>
        {
            return _mapper.Map<CatalogManufacturerDto>(await _catalogManufacturerRepository.UpdateCatalogManufacturerAsync(id, name, foundationYear, headquartersLocation));
        });
    }
}
