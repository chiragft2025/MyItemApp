using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyNewApp.Data;
using MyNewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SerialNumbersController : Controller
    {
        private readonly MyAppContext _context;

        public SerialNumbersController(MyAppContext context)
        {
            _context = context;
        }

        // GET: SerialNumbers
        public async Task<IActionResult> Index()
        {
            var myAppContext = _context.SerialNumbers.Include(s => s.Item);
            return View(await myAppContext.ToListAsync());
        }

        // GET: SerialNumbers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialNumber = await _context.SerialNumbers
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serialNumber == null)
            {
                return NotFound();
            }

            return View(serialNumber);
        }

        // GET: SerialNumbers/Create
        public IActionResult Create()
        {
            // Step 1: Find all ItemIds that already have a SerialNumber
            var usedItemIds = _context.SerialNumbers
                .Select(s => s.ItemId)
                .ToList();

            // Step 2: Get only items that do NOT have a SerialNumber
            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

            // Step 3: Populate dropdown with Item.Name instead of Id
            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name");

            return View();
        }

        // POST: SerialNumbers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ItemId")] SerialNumber serialNumber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serialNumber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate dropdown when validation fails (same filter)
            var usedItemIds = _context.SerialNumbers
                .Select(s => s.ItemId)
                .ToList();

            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name", serialNumber.ItemId);

            return View(serialNumber);
        }
        // GET: SerialNumbers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialNumber = await _context.SerialNumbers.FindAsync(id);
            if (serialNumber == null)
            {
                return NotFound();
            }

            // Step 1: Get all used ItemIds except the current serial's ItemId
            var usedItemIds = _context.SerialNumbers
                .Where(s => s.ItemId != serialNumber.ItemId)
                .Select(s => s.ItemId)
                .ToList();

            // Step 2: Get items that are not used or the current one
            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

            // Step 3: Populate dropdown with Name instead of Id
            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name", serialNumber.ItemId);

            return View(serialNumber);
        }

        // POST: SerialNumbers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ItemId")] SerialNumber serialNumber)
        {
            if (id != serialNumber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serialNumber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SerialNumberExists(serialNumber.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Re-populate dropdown if validation fails
            var usedItemIds = _context.SerialNumbers
                .Where(s => s.ItemId != serialNumber.ItemId)
                .Select(s => s.ItemId)
                .ToList();

            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name", serialNumber.ItemId);

            return View(serialNumber);
        }


        // GET: SerialNumbers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialNumber = await _context.SerialNumbers
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serialNumber == null)
            {
                return NotFound();
            }

            return View(serialNumber);
        }

        // POST: SerialNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serialNumber = await _context.SerialNumbers.FindAsync(id);
            if (serialNumber != null)
            {
                _context.SerialNumbers.Remove(serialNumber);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerialNumberExists(int id)
        {
            return _context.SerialNumbers.Any(e => e.Id == id);
        }
    }
}
