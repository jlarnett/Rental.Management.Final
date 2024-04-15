using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rental.Management.Final.Data;
using Rental.Management.Final.Models;

namespace Rental.Management.Final.Controllers
{
    public class RentalContractsController : Controller
    {
        private readonly ApplicationContext _context;

        public RentalContractsController(ApplicationContext context)
        {
            _context = context;
        }
        // GET: RentalContracts
        public IActionResult Create(int id)
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName");

            var contract = new RentalContract()
            {
                PropertyId = id,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            return View(contract);
        }

        // POST: RentalProperties/Create

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,StartDate,EndDate,PropertyId,CustomerId")] RentalContract contract)
        {

            if (ModelState.IsValid)
            {
                await _context.AddAsync(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }
    }
}
