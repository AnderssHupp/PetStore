namespace Loja.Models
{
    public class Stock : BaseEntity
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
