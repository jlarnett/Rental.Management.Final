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
        // GET: RentalContracts
        public async Task<IActionResult> Create(int id)
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName");
            var activeContracts = await _context.RentalContracts.Where(c => c.PropertyId == id && c.PaymentReceived).ToListAsync();

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

            var contract = new RentalContract()
            {
                PropertyId = id,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,StartDate,EndDate,PropertyId,CustomerId")] RentalContract contract)
        {

            if (ModelState.IsValid)
            {
                var newContract = await _context.AddAsync(contract);
                await _context.SaveChangesAsync();


                var property = await _context.RentalProperties.FindAsync(contract.PropertyId);
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
                return RedirectToAction(nameof(CreateContractPayment));
            }
            return View(payment);
        }
    }


}
