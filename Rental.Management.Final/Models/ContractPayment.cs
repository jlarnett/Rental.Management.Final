namespace Rental.Management.Final.Models
{
    public class ContractPayment
    {
        public int Id { get; set; }
        public int CardHolderName { get; set; }
        public string CardType { get; set; }
        public long CardNumber { get; set; }
        public int CardExpMonth { get; set; }
        public int CardExpYear { get; set; }
        public double Amount { get; set; }

        public int ContractId { get; set; }
        public RentalContract? Contract { get; set; }
    }
}
