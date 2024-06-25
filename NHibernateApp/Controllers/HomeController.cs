using Microsoft.AspNetCore.Mvc;
using NHibernateApp.Models;
using NHibernateApp.Repositories;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace NHibernateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public HomeController(ILogger<HomeController> logger
            , IOrderRepository orderRepository
            , IOrderItemRepository orderItemRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderItems(Guid orderId, string orderNumber)
        {
            var orderItems = await _orderItemRepository.GetOrderItemsAsync(orderId, orderNumber);
            return PartialView("PartialOrderItems", orderItems);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
