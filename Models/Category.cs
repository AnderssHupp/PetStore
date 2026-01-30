namespace Loja.Models
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }

        //navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
