using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rental.Management.Final.Models;

public class RentalProperty
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsOccupied { get; set; } = false;
    public double Price { get; set; }
    public byte[]? Image { get; set; }

    [FromForm]
    [NotMapped]
    public IFormFileCollection? PropertyFiles { get; set; }
    public List<Customer> Customers { get; set; } = new List<Customer>();

}
