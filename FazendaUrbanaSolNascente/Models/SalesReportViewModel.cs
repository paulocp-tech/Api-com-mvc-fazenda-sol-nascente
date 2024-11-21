namespace FazendaUrbanaSolNascente.Models;

public class SalesReportViewModel
{
    public IEnumerable<Sale> Sales { get; set; }
    public decimal TotalVendido { get; set; }
}