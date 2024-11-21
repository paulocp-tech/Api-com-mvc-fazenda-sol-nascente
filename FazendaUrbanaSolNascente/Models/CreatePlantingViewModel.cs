namespace FazendaUrbanaSolNascente.Models;

public class CreatePlantingViewModel
{
    public DateTime DataPlantio { get; set; }
    public DateTime DataColheita { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public List<Product> Produtos { get; set; } // Lista de produtos disponíveis para o plantio

    //public List<Insumo> Insumos { get; set; } // Caso você queira capturar os insumos, adicione a lógica para isso
}
