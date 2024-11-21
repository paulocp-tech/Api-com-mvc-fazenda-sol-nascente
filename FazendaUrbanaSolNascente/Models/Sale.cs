namespace FazendaUrbanaSolNascente.Models;

public class Sale
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalValue { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}