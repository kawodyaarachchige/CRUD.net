using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Models; 
using System.Threading.Tasks;

namespace MyFirstApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<adminModel> _passwordHasher;

        public AdminController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<adminModel>();
        }
        
        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(string username, string email, string password)
        {
            // Check if email already exists
            var existingAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
            if (existingAdmin != null)
            {
                ModelState.AddModelError("", "Email already exists.");
                return View();
            }
            var admin = new adminModel 
            { 
                Username = username,  
                Email = email 
            };
            
            admin.PasswordHash = _passwordHasher.HashPassword(admin, password);
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
            if (admin != null && _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password) == PasswordVerificationResult.Success)
            {
                return RedirectToAction("Index", "Home"); 
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }
    }
}
