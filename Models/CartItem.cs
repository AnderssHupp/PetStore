namespace Loja.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public required Cart Cart { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
