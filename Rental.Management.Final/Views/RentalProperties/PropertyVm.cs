using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Rental.Management.Final.Models;

namespace Rental.Management.Final.Views.RentalProperties
{
    public class PropertyVm
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsOccupied { get; set; } = false;

        [DisplayName("Daily Fee")]
        public double Price { get; set; }
        public byte[]? Image { get; set; }

        public List<byte[]>? PropertyImages { get; set; }

        [FromForm]
        [NotMapped]
        public IFormFileCollection? PropertyFiles { get; set; }
        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
