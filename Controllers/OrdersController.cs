using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Models;

namespace MyFirstApp.Controllers;

    public class OrdersController : Controller {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var orders = _context.Orders.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.Item).ToList();
            return View(orders);
        }

        public IActionResult Create() {
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Items = _context.Items.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int userId, List<int> itemIds, List<int> quantities) {
            var order = new OrderModel() {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = 0
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            for (int i = 0; i < itemIds.Count; i++) {
                var item = _context.Items.Find(itemIds[i]);
                var orderItem = new OrderItemodel() {
                    OrderId = order.Id,
                    ItemId = item.Id,
                    Quantity = quantities[i],
                    Price = item.Price
                };
                order.TotalAmount += item.Price * quantities[i];
                item.Quantity -= quantities[i];
                _context.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

