using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rental.Management.Final.Models;

public class Customer
{
    public int Id { get; set; }

    [MaxLength(50)] 
    [DisplayName("First Name")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50)] 
    [DisplayName("Last Name")]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(100)] 
    [DisplayName("Address")]
    public string Address { get; set; } = string.Empty;

    [Phone]
    [DisplayName("Phone")]
    public string PhoneNumber { get; set; } = string.Empty;
}
