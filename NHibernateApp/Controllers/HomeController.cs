using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using NHibernateApp.Models;
using NHibernateApp.Repositories;
using Remotion.Linq.Clauses;
using System.Diagnostics;

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
        [HttpPost]
        [Route("api/Home/AddOrUpdateOrder")]
        public async Task<IActionResult> AddOrUpdateOrder(Order order, bool isUpdate)
        {
            try
            {
                var res = await _orderRepository.AddOrderAndItemsAsync(order);
                if (!res) return Json(new ApiResponse("Order number exists!"));

                return Json(new ApiResponse());
                //handle add order and order items
                //if (!isUpdate)
                //{
                //    var res = await _orderRepository.AddOrderAndItemsAsync(order);
                //    if (!res) return Json(new ApiResponse("Order number exists!"));

                //    return Json(new ApiResponse());
                //}

                //handle update order and order items
                //var resUpdateOrder = await _orderRepository.UpdateAsync(order);
                //if (resUpdateOrder == null) return Json(new ApiResponse("This order no longer exists"));

                //var resUpdateItems = await _orderItemRepository.AddOrUpdateItemsAsync(order.OrderItems, order.Id);
                //if (!resUpdateItems) return Json(new ApiResponse("Something occurs error!"));

                //return Ok(new ApiResponse());
            }
            catch (Exception ex) {
                return Json(new ApiResponse(ex.Message));
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderItemsForm(Guid orderId)
        {
            var orderItems = await _orderItemRepository.GetOrderItemsAsync(orderId);
            return PartialView("PartialOrderItemsForm", orderItems);
        }
        [HttpPost]
        [Route("api/Home/DeleteOrdersOrItems")]
        public async Task<IActionResult> DeleteOrdersOrItems(List<Guid> guidIds, bool isDeleteOrder)
        {
            try
            {
                if(guidIds == null || !guidIds.Any()) return Json(new ApiResponse("Missing data"));
                if (isDeleteOrder)
                {
                    var res =  await _orderRepository.DeleteAsync(guidIds);
                    if(!res) return Json(new ApiResponse("Something occurs error!"));
                }
                else
                {
                    var res = await _orderItemRepository.DeleteAsync(guidIds);
                    if (!res) return Json(new ApiResponse("Something occurs error!"));
                }
                return Ok(new ApiResponse());
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse(ex.Message));
            }
        }
        [HttpPost]
        [Route("api/Home/UpdateOrderItem")]
        public async Task<IActionResult> UpdateOrderItem(OrderItem orderItem)
        {
            try
            {
                var res = _orderItemRepository.Update(orderItem);
                
                if(res == null) return Json(new ApiResponse("This item no longer exists"));

                return Ok(new ApiResponse());
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse(ex.Message));
            }
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
