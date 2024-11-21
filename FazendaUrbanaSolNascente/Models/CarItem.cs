namespace FazendaUrbanaSolNascente.Models;

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
}