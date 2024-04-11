using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental.Management.Final.Data;
using Rental.Management.Final.Models;

namespace Rental.Management.Final.Controllers
{
    public class RentalPaymentsController : Controller
    {
        private readonly ApplicationContext _context;

        public RentalPaymentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: RentalPayments
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.RentalPayments.Include(r => r.Customer).Include(r => r.Property);
            return View(await applicationContext.ToListAsync());
        }

        // GET: RentalPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RentalPayments == null)
            {
                return NotFound();
            }

            var rentalPayment = await _context.RentalPayments
                .Include(r => r.Customer)
                .Include(r => r.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalPayment == null)
            {
                return NotFound();
            }

            return View(rentalPayment);
        }

        // GET: RentalPayments/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["RentalPropertyId"] = new SelectList(_context.RentalProperties, "Id", "Description");
            return View();
        }

        // POST: RentalPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,RentalPropertyId,PaymentAmount,PaymentDate")] RentalPayment rentalPayment)
        {
            if (ModelState.IsValid)
            {
                rentalPayment.PaymentDate = DateTime.UtcNow;
                _context.Add(rentalPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", rentalPayment.CustomerId);
            ViewData["RentalPropertyId"] = new SelectList(_context.RentalProperties, "Id", "Id", rentalPayment.RentalPropertyId);
            return View(rentalPayment);
        }

        // GET: RentalPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentalPayments == null)
            {
                return NotFound();
            }

            var rentalPayment = await _context.RentalPayments.FindAsync(id);
            if (rentalPayment == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", rentalPayment.CustomerId);
            ViewData["RentalPropertyId"] = new SelectList(_context.RentalProperties, "Id", "Id", rentalPayment.RentalPropertyId);
            return View(rentalPayment);
        }

        // POST: RentalPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,RentalPropertyId,PaymentAmount,PaymentDate")] RentalPayment rentalPayment)
        {
            if (id != rentalPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalPaymentExists(rentalPayment.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", rentalPayment.CustomerId);
            ViewData["RentalPropertyId"] = new SelectList(_context.RentalProperties, "Id", "Id", rentalPayment.RentalPropertyId);
            return View(rentalPayment);
        }

        // GET: RentalPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RentalPayments == null)
            {
                return NotFound();
            }

            var rentalPayment = await _context.RentalPayments
                .Include(r => r.Customer)
                .Include(r => r.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalPayment == null)
            {
                return NotFound();
            }

            return View(rentalPayment);
        }

        // POST: RentalPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RentalPayments == null)
            {
                return Problem("Entity set 'ApplicationContext.RentalPayments'  is null.");
            }
            var rentalPayment = await _context.RentalPayments.FindAsync(id);
            if (rentalPayment != null)
            {
                _context.RentalPayments.Remove(rentalPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalPaymentExists(int id)
        {
          return (_context.RentalPayments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
