﻿using MVC.Services.Interfaces;
using MVC.ViewModels.CatalogViewModels;
using MVC.ViewModels.Pagination;

namespace MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ICatalogService catalogService, ILogger<CatalogController> logger)
    {
        _catalogService = catalogService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int? manufacturersFilterApplied, int? page, int? itemsPage)
    {   
        page ??= 0;
        itemsPage ??= 5;

        _logger.LogInformation($"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA Page = {page}, itemsPage = {itemsPage}, ManufacturerFilterApplied = {manufacturersFilterApplied}");
        
        var catalog = await _catalogService.GetCatalogCars(page.Value, itemsPage.Value, manufacturersFilterApplied);
        if (catalog == null)
        {
            return View("Error");
        }
        var info = new PaginationInfo()
        {
            ActualPage = page.Value,
            ItemsPerPage = catalog.Data.Count,
            TotalItems = catalog.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage.Value)
        };
        var vm = new IndexViewModel()
        {
            CatalogCars = catalog.Data,
            CatalogManufacturers = await _catalogService.GetCatalogManufacturers(),
            PaginationInfo = info,
            ManufacturerFilterApplied = manufacturersFilterApplied
        };

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

        return View(vm);
    }
}