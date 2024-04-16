using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rental.Management.Final.Models
{
    public class ContractPayment
    {
        public int Id { get; set; }

        [DisplayName("Card Holder")]
        public string CardHolderName { get; set; }

        [DisplayName("Card Type")]
        public string CardType { get; set; }

        [MaxLength(16)]
        [MinLength(16)]
        [DisplayName("Credit Card Number")]
        public string CardNumber { get; set; }

        [DisplayName("Expiration Month")]
        [MinLength(2)]
        [MaxLength(2)]
        public string CardExpMonth { get; set; }

        [DisplayName("Expiration Year")]
        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string CardExpYear { get; set; }

        public double Amount { get; set; }

        public int ContractId { get; set; }
        public RentalContract? Contract { get; set; }
        public DateTime Date { get; set; }

        [DisplayName("Address")]
        public string BillingAddress { get; set; } = string.Empty;

        [DisplayName("City")]
        public string BillingCity { get; set; } = string.Empty;

        [DisplayName("Zip Code")]
        public string BillingZipCode { get; set; } = string.Empty;
    }
}
