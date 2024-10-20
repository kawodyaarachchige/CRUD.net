using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CreateOrder(int userId, List<int> itemIds, List<int> quantities)
        {
            // Debugging: Log the received data
            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine($"Item IDs: {string.Join(", ", itemIds)}");
            Console.WriteLine($"Quantities: {string.Join(", ", quantities)}");

      
            if (itemIds == null || quantities == null || itemIds.Count == 0 || quantities.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one item.");
                return View();
            }

            var order = new OrderModel()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = 0,
                OrderItems = new List<OrderItemModel>()
            };

            for (int i = 0; i < itemIds.Count; i++)
            {
                var item = await _context.Items.FindAsync(itemIds[i]);
                if (item != null && quantities[i] > 0)
                {
                    if (item.Quantity >= quantities[i])
                    {
                        var orderItem = new OrderItemModel()
                        {
                            ItemId = item.Id,
                            Quantity = quantities[i],
                            Price = item.Price 
                        };
                        order.TotalAmount += item.Price * quantities[i];

                        item.Quantity -= quantities[i]; 
                        order.OrderItems.Add(orderItem); 
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Not enough stock for item: {item.Name}");
                    }
                }
            }

            if (!order.OrderItems.Any())
            {
                ModelState.AddModelError("", "No valid items were selected.");
                return View();
            }
    
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync(); 
    
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = order.Id; 
                await _context.OrderItems.AddAsync(orderItem); 
            }
    
            return RedirectToAction("Index");
        }


    }
}
