using System.ComponentModel.DataAnnotations.Schema;

namespace Rental.Management.Final.Models
{
    public class RentalPayment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int RentalPropertyId { get; set; }
        public RentalProperty? Property { get; set; }
        [Column(TypeName = "money")] public double PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}