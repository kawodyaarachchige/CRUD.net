using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Models;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            CustomerCount = await _context.Users.CountAsync(),
            ItemCount = await _context.Items.CountAsync(),
            OrderCount = await _context.Orders.CountAsync()
        };

        return View(viewModel);
    }
   
 

}