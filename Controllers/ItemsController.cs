using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyNewApp.Data;
using MyNewApp.Models;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace MyNewApp.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly MyAppContext _context;

        public ItemsController(MyAppContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {

            var items = await _context.Items.Include(s => s.SerialNumber)
                                            .Include(i => i.Category)
                                            .Include(ic => ic.ItemClients)
                                            .ThenInclude(c => c.Client)
                                                    .ToListAsync();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> AddItems()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddItems([Bind("Id,Name,Price,CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetItems");
            }

            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult> EditItem(int id)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");

            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> EditItem([Bind("Id,Name,Price,CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetItems");
            }
            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }
        [HttpPost, ActionName("RemoveItem")]
        public async Task<IActionResult> RemoveItemconfirm(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetItems");
            }
            return View();
        }

        public async Task<IActionResult> GetSIngleItem(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);
            return View(item);
        }



    }
}
