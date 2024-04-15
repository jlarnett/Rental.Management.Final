using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public RentalPropertiesController(ApplicationContext context, IFileExtensionValidator validator, IMapper mapper)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
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

            var propertyVm = await MapToPropertyVm(rentalProperty);

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

            var vm = await MapToPropertyVm(rentalProperty);

            return View(vm);
        }

        // POST: RentalProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Address,IsOccupied,Price,PropertyFiles")] PropertyVm rentalProperty)
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

                    await DeleteOldPropertyImagesFromDb(id);
                    await SaveImages(id, rentalProperty.PropertyFiles!);

                    var property = MapToRentalProperty(rentalProperty);

                    _context.Update(property);
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

        /// <summary>
        /// Removes all the old images from property. Used when editing property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        private async Task DeleteOldPropertyImagesFromDb(int propertyId)
        {
            var images = await _context.PropertyImages.Where(c => c.PropertyId.Equals(propertyId)).ToListAsync();
            _context.RemoveRange(images);
            await _context.SaveChangesAsync();
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
                var images = _context.PropertyImages.Where(i => i.PropertyId.Equals(id)).ToList();
                _context.PropertyImages.RemoveRange(images);
                await _context.SaveChangesAsync();

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

        private async Task<PropertyVm> MapToPropertyVm(RentalProperty rentalProperty)
        {
            var propertyImages = await _context.PropertyImages.Where(p => p.PropertyId.Equals(rentalProperty.Id))
                .ToListAsync();
            var images = new List<byte[]>();

            foreach (var image in propertyImages)
                images.Add(image.Image);

            var vm = new PropertyVm()
            {
                Id = rentalProperty.Id,
                Address = rentalProperty.Address,
                Description = rentalProperty.Description,
                Customers = rentalProperty.Customers,
                Price = rentalProperty.Price,
                IsOccupied = rentalProperty.IsOccupied,
                PropertyFiles = rentalProperty.PropertyFiles,
                PropertyImages = images
            };

            return vm;
        }

        private RentalProperty MapToRentalProperty(PropertyVm rentalProperty)
        {
            var property = new RentalProperty()
            {
                Id = rentalProperty.Id,
                Address = rentalProperty.Address,
                Description = rentalProperty.Description,
                Customers = rentalProperty.Customers,
                Price = rentalProperty.Price,
                IsOccupied = rentalProperty.IsOccupied,
                PropertyFiles = rentalProperty.PropertyFiles,
            };

            return property;
        }
    }
}
