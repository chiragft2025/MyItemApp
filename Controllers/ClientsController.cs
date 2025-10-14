using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNewApp.Data;
using MyNewApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNewApp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MyAppContext _context;

        public ClientsController(MyAppContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.ItemClients)
                .ThenInclude(ic => ic.Item)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Client client)
        {
            if (id != client.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null) return NotFound();

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================== Many-to-Many: Assign Items to Client ==================

        // GET: Clients/EditItems/5
        public async Task<IActionResult> EditItems(int id)
        {
            var client = await _context.Clients
                .Include(c => c.ItemClients)
                .ThenInclude(ic => ic.Item)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            ViewBag.AllItems = await _context.Items.ToListAsync();
            ViewBag.SelectedItemIds = client.ItemClients.Select(ic => ic.ItemId).ToList();
            ViewBag.ClientName = client.Name;

            return View(client);
        }

        // POST: Clients/EditItems/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItems(int id, List<int> selectedItemIds)
        {
            var client = await _context.Clients
                .Include(c => c.ItemClients)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            // Remove all existing item links
            client.ItemClients.Clear();

            // Add new links
            if (selectedItemIds != null)
            {
                foreach (var itemId in selectedItemIds)
                {
                    client.ItemClients.Add(new ItemClient
                    {
                        ClientId = id,
                        ItemId = itemId
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        // ================== Helper ==================
        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
