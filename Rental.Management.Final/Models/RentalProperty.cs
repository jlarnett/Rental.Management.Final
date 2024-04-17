using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Rental.Management.Final.Models;

public class RentalProperty
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

    [FromForm]
    [NotMapped]
    public IFormFileCollection? PropertyFiles { get; set; }
    public List<Customer> Customers { get; set; } = new List<Customer>();

}
