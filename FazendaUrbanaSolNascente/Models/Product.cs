namespace FazendaUrbanaSolNascente.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int QtdStorage { get; set; }
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}