using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var contracts = await _context.RentalContracts.Include(c => c.Customer).Include(c => c.RentalProperty)
                .ToListAsync();
            return _context.RentalContracts != null ? 
                View(contracts) :
                Problem("Entity set 'ApplicationContext.RentalContracts'  is null.");
        }

        public async Task<IActionResult> Create(int id)
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName");
            var activeContracts = await _context.RentalContracts.Where(c => c.RentalPropertyId == id && c.PaymentReceived).ToListAsync();

            List<DateTime> disabledDates = new List<DateTime>();

            foreach(var activeContract in activeContracts)
            {
                var startingDate = activeContract.StartDate;

                while(startingDate <= activeContract.EndDate)
                {
                    disabledDates.Add(startingDate);
                    startingDate = startingDate.AddDays(1);
                }
            }

            string dDates = string.Join(",", disabledDates.Select(c => c.ToString("yyyy-MM-dd")).ToArray());
            ViewBag.DisabledDates = dDates;

            ViewData["RentalPropertyId"] = new SelectList(_context.RentalProperties, "Id", "Description");

            if (id == 0)
                ViewBag.PropertyIdSupplied = false;
            else
                ViewBag.PropertyIdSupplied = false;

            var contract = new RentalContract()
            {
                RentalPropertyId = id,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,StartDate,EndDate,RentalPropertyId,CustomerId")] RentalContract contract)
        {

            if (ModelState.IsValid)
            {
                var newContract = await _context.AddAsync(contract);
                await _context.SaveChangesAsync();


                var property = await _context.RentalProperties.FindAsync(contract.RentalPropertyId);
                var totalDaysForContract = contract.EndDate.Subtract(contract.StartDate).Days;
                return RedirectToAction(nameof(CreateContractPayment), new {id=newContract.Entity.ContractId, numberOfDaysReserved=totalDaysForContract, propertyPrice=property.Price});
            }
            return View(contract);
        }

        // GET: RentalContracts
        public IActionResult CreateContractPayment(int id, int numberOfDaysReserved, double propertyPrice)
        {
            ViewBag.DaysInContract = numberOfDaysReserved;
            ViewBag.PropertyPrice = propertyPrice;

            return View("Payment", new ContractPayment()
            {
                Date = DateTime.UtcNow,
                Amount = propertyPrice * numberOfDaysReserved,
                ContractId = id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContractPayment([Bind("CardHolderName,CardType,CardNumber,CardExpMonth,CardExpYear,Amount,ContractId,Date,BillingAddress,BillingCity,BillingZipCode")] ContractPayment payment)
        {

            if (ModelState.IsValid)
            {
                await _context.AddAsync(payment);
                var rowsModified = await _context.SaveChangesAsync();

                if (rowsModified > 0)
                {
                    var contract = await _context.RentalContracts.FindAsync(payment.ContractId);

                    if (contract != null)
                    {
                        contract.PaymentReceived = true;
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(PaymentConfirmation));
            }
            return View(payment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentalContracts == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.RentalContracts.FindAsync(id);

            if (rentalContract == null)
            {
                return NotFound();
            }
            ViewBag.PropertyIdSupplied = true;

            return View(rentalContract);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RentalProperties == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.RentalContracts
                .FirstOrDefaultAsync(m => m.ContractId == id);

            if (rentalContract == null)
            {
                return NotFound();
            }

            return View(rentalContract);
        }

        // POST: RentalProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RentalContracts == null)
            {
                return Problem("Entity set 'ApplicationContext.RentalContracts'  is null.");
            }
            var rentalContract = await _context.RentalContracts.FindAsync(id);

            if (rentalContract == null)
            {
                return NotFound();
            }
            //Remove old payments connected to contract before deleting contract
            var connectedPayments = await _context.ContractPayments.Where(c => c.ContractId.Equals(id)).ToListAsync();
            _context.ContractPayments.RemoveRange(connectedPayments);
            await _context.SaveChangesAsync();

            _context.RentalContracts.Remove(rentalContract); 
            var rowsChanged = await _context.SaveChangesAsync();

            if (rowsChanged > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Delete");
        }

        public async Task<IActionResult> PaymentConfirmation(int id)
        {
            return View("PaymentConfirmation");
        }
    }
}
