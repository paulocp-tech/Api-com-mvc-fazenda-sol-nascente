namespace FazendaUrbanaSolNascente.Models;

public class Planting
{
    public int Id { get; set; }
    public DateTime PlantingDate { get; set; }

    public DateTime HarvestDate { get; set; }

    //public List<Supply> Supplies { get; set; }

    public int ProductId { get; set; }
    public int Quantidade { get; set; }
    public Product Produto { get; set; }
}