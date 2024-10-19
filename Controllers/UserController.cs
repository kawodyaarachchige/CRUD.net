using Microsoft.AspNetCore.Mvc;
using MyFirstApp.Models;
using Microsoft.EntityFrameworkCore; // Required for EF Core functionalities
using System.Linq;
using System.Threading.Tasks;


public class UsersController : Controller {
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context) {
        _context = context;
    }

    public IActionResult Index() {
        var users = _context.Users.ToList();
        return View(users);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(UserModel user) {
        _context.Users.Add(user);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id) {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public IActionResult Edit(UserModel user) {
        _context.Users.Update(user);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id) {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id) {
        var user = _context.Users.Find(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
