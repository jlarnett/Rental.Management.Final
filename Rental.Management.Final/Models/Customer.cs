namespace Rental.Management.Final.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RentalPropertyId { get; set; }
    public RentalProperty? RentalProperty { get; set; }
}
