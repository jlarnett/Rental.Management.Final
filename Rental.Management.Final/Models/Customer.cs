using System.ComponentModel.DataAnnotations;

namespace Rental.Management.Final.Models;

public class Customer
{
    public int Id { get; set; }

    [MaxLength(50)] public string FirstName { get; set; } = string.Empty;
    [MaxLength(50)] public string LastName { get; set; } = string.Empty;
    [MaxLength(100)] public string Address { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}
