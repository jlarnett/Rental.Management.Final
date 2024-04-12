using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Rental.Management.Final.Data;
using Rental.Management.Final.Models;

namespace Rental.Management.Final.Controllers
{
    public class RentalPropertiesController : Controller
    {
        private readonly ApplicationContext _context;

        public RentalPropertiesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: RentalProperties
        public async Task<IActionResult> Index()
        {
              return _context.RentalProperties != null ? 
                          View(await _context.RentalProperties.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.RentalProperties'  is null.");
        }

        // GET: RentalProperties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RentalProperties == null)
            {
                return NotFound();
            }

            var rentalProperty = await _context.RentalProperties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalProperty == null)
            {
                return NotFound();
            }

            return View(rentalProperty);
        }

        // GET: RentalProperties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RentalProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Address,IsOccupied,Price,PropertyFiles")] RentalProperty rentalProperty)
        {
            if (ModelState.IsValid)
            {
                using (var ms = new MemoryStream())
                {
                    rentalProperty.PropertyFiles.First().CopyTo(ms);
                    rentalProperty.Image = ms.ToArray();
                }
                _context.Add(rentalProperty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rentalProperty);
        }

        // GET: RentalProperties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentalProperties == null)
            {
                return NotFound();
            }

            var rentalProperty = await _context.RentalProperties.FindAsync(id);

            if (rentalProperty == null)
            {
                return NotFound();
            }
            return View(rentalProperty);
        }

        // POST: RentalProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Address,IsOccupied,Price")] RentalProperty rentalProperty)
        {
            if (id != rentalProperty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalProperty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalPropertyExists(rentalProperty.Id))
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
            return View(rentalProperty);
        }

        // GET: RentalProperties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RentalProperties == null)
            {
                return NotFound();
            }

            var rentalProperty = await _context.RentalProperties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalProperty == null)
            {
                return NotFound();
            }

            return View(rentalProperty);
        }

        // POST: RentalProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RentalProperties == null)
            {
                return Problem("Entity set 'ApplicationContext.RentalProperties'  is null.");
            }
            var rentalProperty = await _context.RentalProperties.FindAsync(id);
            if (rentalProperty != null)
            {
                _context.RentalProperties.Remove(rentalProperty);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalPropertyExists(int id)
        {
          return (_context.RentalProperties?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
