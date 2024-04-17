using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Rental.Management.Final.Models;

namespace Rental.Management.Final.Views.RentalProperties
{
    public class PropertyVm
    {
        public int Id { get; set; }

        [DisplayName("Property Description")]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Property Address")]
        public string Address { get; set; } = string.Empty;

        [DisplayName("Currently Occupied")]
        public bool IsOccupied { get; set; } = false;

        [DisplayName("Daily Fee")]
        public double Price { get; set; }

        public List<byte[]>? PropertyImages { get; set; }

        [FromForm]
        [NotMapped]
        public IFormFileCollection? PropertyFiles { get; set; }
        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
