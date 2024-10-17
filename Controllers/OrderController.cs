using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Models;
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

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Customers = _context.Users.ToList();
            ViewBag.Items = _context.Items.ToList();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, List<OrderItem> orderItems)
        {
            if (ModelState.IsValid && orderItems != null && orderItems.Count > 0)
            {
                order.Total = orderItems.Sum(oi => oi.TotalAmount);
                order.Amount = order.Total;

                // Save order first to get OrderId
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in orderItems)
                {
                    item.OrderId = order.OrderId; // Link order item to the order
                    _context.OrderItems.Add(item);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = _context.Users.ToList();
            ViewBag.Items = _context.Items.ToList();
            return View(order);
        }

        // Add an Index method to display orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.Customer).ToListAsync();
            return View(orders);
        }
    }
}