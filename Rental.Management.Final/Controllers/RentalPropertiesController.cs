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
using Rental.Management.Final.Services.FileExtensionValidator;
using Rental.Management.Final.Views.RentalProperties;

namespace Rental.Management.Final.Controllers
{
    public class RentalPropertiesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IFileExtensionValidator _validator;

        public RentalPropertiesController(ApplicationContext context, IFileExtensionValidator validator)
        {
            _context = context;
            _validator = validator;
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
            var propertyImages = await _context.PropertyImages.Where(p => p.PropertyId.Equals(id)).ToListAsync();
            List<byte[]> images = new List<byte[]>();
            foreach (var image in propertyImages)
            {
                images.Add(image.Image);
            }

            var propertyVm = new PropertyVm()
            {
                Id = rentalProperty.Id,
                Address = rentalProperty.Address,
                Description = rentalProperty.Description,
                Price = rentalProperty.Price,
                IsOccupied = rentalProperty.IsOccupied,
                Customers = rentalProperty.Customers,
                PropertyImages = images,
            };

            if (rentalProperty == null)
            {
                return NotFound();
            }

            return View(propertyVm);
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
                await _context.AddAsync(rentalProperty);
                await _context.SaveChangesAsync();

                var newRental = await _context.RentalProperties.Where(r =>
                        r.Description.Equals(rentalProperty.Description) && r.Address.Equals(rentalProperty.Address))
                    .FirstAsync();

                //Validate the file extensions to verify they are images
                if (rentalProperty.PropertyFiles != null)
                {
                    foreach (var file in rentalProperty.PropertyFiles)
                    {
                        if (_validator.CheckValidImageExtensions(file.FileName) != true)
                            return BadRequest();
                    }
                }

                await SaveImages(newRental.Id, rentalProperty.PropertyFiles!);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Address,IsOccupied,Price,PropertyFiles")] RentalProperty rentalProperty)
        {
            if (id != rentalProperty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Validate the file extensions to verify they are images
                    if (rentalProperty.PropertyFiles != null)
                    {
                        foreach (var file in rentalProperty.PropertyFiles)
                        {
                            if (_validator.CheckValidImageExtensions(file.FileName) != true)
                                return BadRequest();
                        }
                    }

                    await SaveImages(id, rentalProperty.PropertyFiles!);
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

        private async Task SaveImages(int propertyId, IFormFileCollection files)
        {

            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                await _context.PropertyImages.AddAsync(new PropertyImage()
                {
                    Image = ms.ToArray(),
                    PropertyId = propertyId
                });
            }


            await _context.SaveChangesAsync();
        }
    }
}
