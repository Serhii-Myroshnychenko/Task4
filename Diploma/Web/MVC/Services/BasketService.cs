﻿using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Models.Items;
using Infrastructure.Models.Requests;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<BasketService> _logger;
        public BasketService(
            IMapper mapper,
            IOptions<AppSettings> settings,
            IHttpClientService httpClient,
            ILogger<BasketService> logger)
        {
            _mapper = mapper;
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task AddToBasket(CatalogCar car)
        {
            _logger.LogInformation($"Added new item to basket with the following parameters: id: {car.Id}");
            var items = await GetBasketItems();
            var listOfItems = items.ToList();

            if (items.Any(c => c.Id == car.Id))
            {
                foreach (var elem in items.Where(c => c.Id == car.Id))
                {
                    if(elem.Quantity < car.Quantity)
                    {
                        elem.Quantity++;
                    }
                }
            }
            else
            {
                var mappedElem = _mapper.Map<CatalogBasketCar>(car);
                mappedElem.Quantity = 1;
                listOfItems.Add(mappedElem);
            }

            await UpdateBasket(listOfItems);
            _logger.LogInformation($"Updated basket.");
        }

        public async Task<IEnumerable<CatalogBasketCar>> GetBasketItems()
        {
            _logger.LogInformation($"GetBasketItems method executed.");
            var result = await _httpClient.SendAsync<GroupedEntities<CatalogBasketCar>,object>
                ($"{_settings.Value.BasketUrl}/GetBasket",
                HttpMethod.Post, null);
            return result.Data;
        }

        public async Task RemoveFromBasket(CatalogBasketCar car)
        {
            _logger.LogInformation($"RemoveFromBasket method executed.");
            var currentBasket = await GetBasketItems();
            if (currentBasket.Any(c=> c.Id == car.Id))
            {
                var updatedBasket = currentBasket.Where(c => c.Id != car.Id);
                await UpdateBasket(updatedBasket);
            }
        }

        public async Task UpdateBasket(IEnumerable<CatalogBasketCar> cars)
        {
            _logger.LogInformation($"UpdateBasket method executed.");
            await _httpClient.SendAsync<GroupedEntities<CatalogBasketCar>, AddToBasketRequest>
                ($"{_settings.Value.BasketUrl}/AddCarsToBasket",
                HttpMethod.Post, new AddToBasketRequest() { Data = cars.ToList() });
        }
    }
}
