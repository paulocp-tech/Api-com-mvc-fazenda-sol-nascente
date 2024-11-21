namespace FazendaUrbanaSolNascente.Models;

public class CreateSaleViewModel
{
    public List<Product> Produtos { get; set; } = new List<Product>();
    public int SelectedProductId { get; set; }
    public List<CartItem> Cart { get; set; } = new List<CartItem>();
    public List<Customer> Customer { get; set; } = new List<Customer>();
}