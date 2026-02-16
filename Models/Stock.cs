namespace Loja.Models
{
    public class Stock : BaseEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }

    }
}
