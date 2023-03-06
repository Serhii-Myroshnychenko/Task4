﻿using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.ViewModels.OrderViewModels;

namespace MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orderService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _identityParser = identityParser;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var user = _identityParser.Parse(User);
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var items = await _orderService.GetOrderById();

            Console.WriteLine($"Iteeeemsss ORDEERRRRR: {items.Count()}");

            var total = items.Sum(c => c.Price * c.Quantity);

            var orderViewModel = new OrderViewModel()
            {
                CatalogBasketCars = items,
                Total = total
            };

            return View(orderViewModel);
        }
        public async Task<IActionResult> PlaceOrder()
        {
            var user = _identityParser.Parse(User);
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            await _orderService.PlaceOrder(user.Name, user.Name);
            return RedirectToAction(nameof(CatalogController.Index), "Catalog");
        }
    }
}
