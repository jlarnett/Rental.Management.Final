﻿using Microsoft.AspNetCore.Mvc;
using Rental.Management.Final.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Rental.Management.Final.Data;
using Rental.Management.Final.Views.Home;
using Rental.Management.Final.Views.RentalProperties;
using Hangfire;

namespace Rental.Management.Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            RecurringJob.AddOrUpdate("OccupiedRentalCheck",() => CheckRentalStatus(),Cron.Hourly);
            var properties = await _context.RentalProperties.Where(p => p.IsOccupied != true).ToListAsync();

            List<PropertyVm> propertiesVms = new List<PropertyVm>();

            foreach (var property in properties)
            {
                var propertyImages = await _context.PropertyImages.Where(p => p.PropertyId.Equals(property.Id)).ToListAsync();
                List<byte[]> images = new List<byte[]>();
                foreach (var image in propertyImages)
                {
                    images.Add(image.Image);
                }

                var vm = new PropertyVm()
                {
                    Id = property.Id,
                    Address = property.Address,
                    Description = property.Description,
                    Price = property.Price,
                    IsOccupied = property.IsOccupied,
                    Customers = property.Customers,
                    PropertyImages = images,
                };

                propertiesVms.Add(vm);
            }

            propertiesVms = RandomizePropertyList(propertiesVms);

            var IndexVm = new HomeVm()
            {
                DisplayProperties = propertiesVms
            };

            return View(IndexVm);
        }

        public async Task CheckRentalStatus()
        {
            var properties = await _context.RentalProperties.ToListAsync();
            foreach (var property in properties)
            {
                var contracts = await _context.RentalContracts.Where(c => c.RentalPropertyId.Equals(property.Id) && c.PaymentReceived).ToListAsync();
                var isOccupied = false;

                foreach (var contract in contracts)
                {
                    if(DateTime.UtcNow.Ticks > contract.StartDate.Ticks && DateTime.UtcNow.Ticks < contract.EndDate.Ticks)
                    {
                        isOccupied = true;
                    }
                }

                property.IsOccupied = isOccupied;
                _context.Update(property);
            }

            await _context.SaveChangesAsync();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<PropertyVm> RandomizePropertyList(List<PropertyVm> listToShuffle)
        {
            var shuffledList = listToShuffle.OrderBy(_ => Guid.NewGuid()).ToList();
            return shuffledList;
        }
    }
}