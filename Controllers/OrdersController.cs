using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Models;

namespace MyFirstApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToList();
            return View(orders);
        }

        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Items = _context.Items.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int userId, List<int> itemIds, List<int> quantities)
        {
            if (itemIds == null || quantities == null || itemIds.Count == 0 || quantities.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one item.");
                return View();
            }

            var order = new OrderModel()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = 0
            };

            var orderItems = new List<OrderItemModel>();

            for (int i = 0; i < itemIds.Count; i++)
            {
                var item = _context.Items.Find(itemIds[i]);
                if (item != null && quantities[i] > 0)
                {
                    var orderItem = new OrderItemModel()
                    {
                        ItemId = item.Id,
                        Quantity = quantities[i],
                        Price = item.Price
                    };

                    order.TotalAmount += item.Price * quantities[i];
                    item.Quantity -= quantities[i];
                    orderItems.Add(orderItem);
                }
            }

            if (orderItems.Count == 0)
            {
                ModelState.AddModelError("", "No valid items were selected.");
                return View();
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id;
                _context.OrderItems.Add(orderItem);
            }


            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}