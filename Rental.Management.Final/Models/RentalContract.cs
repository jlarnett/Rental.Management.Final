using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rental.Management.Final.Models
{
    public class RentalContract
    {
        //[Key]
        //public int Id { get; set; }

        [Key]
        public int ContractId { get; set; }


        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        public int PropertyId { get; set; }
        public RentalProperty? RentalProperty { get; set; }

        [DisplayName("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public bool PaymentReceived { get; set; } = false;

    }
}
