namespace Rental.Management.Final.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public RentalProperty Property { get; set; }
        public byte[] Image { get; set; }
    }
}
