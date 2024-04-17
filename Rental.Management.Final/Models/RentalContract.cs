using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Rental.Management.Final.Models
{
    public class RentalContract
    {
        //[Key]
        //public int Id { get; set; }

        [Key]
        public int ContractId { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [AllowNull]
        public int? RentalPropertyId { get; set; }

        [AllowNull]
        public RentalProperty? RentalProperty { get; set; }

        [DisplayName("Customer")]
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public bool PaymentReceived { get; set; } = false;
    }
}
