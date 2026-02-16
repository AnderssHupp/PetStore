namespace Loja.Models
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public required decimal Price { get; set; }
        public Stock? Stock { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
