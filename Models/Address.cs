namespace Loja.Models
{
    public class Address : BaseEntity
    {
        public int UserId { get; set; }
        public  required User User { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public required string Country { get; set; }
        public bool IsDefault { get; set; } = true;
    }
}