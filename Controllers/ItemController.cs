using Microsoft.AspNetCore.Mvc;
using MyFirstApp.Models;

public class ItemsController : Controller {
    private readonly AppDbContext _context;

    public ItemsController(AppDbContext context) {
        _context = context;
    }

    public IActionResult Index() {
        var items = _context.Items.ToList();
        return View(items);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Item item) {
        _context.Items.Add(item);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id) {
        var item = _context.Items.Find(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    public IActionResult Edit(Item item) {
        _context.Items.Update(item);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id) {
        var item = _context.Items.Find(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id) {
        var item = _context.Items.Find(id);
        _context.Items.Remove(item);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}