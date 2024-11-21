using FazendaUrbanaSolNascente.Context;
using FazendaUrbanaSolNascente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FazendaUrbanaSolNascente.Controllers;

public class PlantingController : Controller
{
    private readonly SolNascerDbContext _context;

    public PlantingController(SolNascerDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var plantios = _context.Plantings.Include(p => p.Produto).ToList();
        return View(plantios); // Passa a lista de plantios para a view
    }

    public IActionResult Create()
    {
        var viewModel = new CreatePlantingViewModel
        {
            Produtos = _context.Produtos.ToList(),
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePlantingViewModel plantio)
    {
        var planting = new Planting
        {
            PlantingDate = plantio.DataPlantio,
            HarvestDate = plantio.DataColheita,
            ProductId = plantio.ProdutoId,
            Quantidade = plantio.Quantidade,
        };

        _context.Plantings.Add(planting);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var planting = _context.Plantings.FirstOrDefault(p => p.Id == id);
        if (planting == null)
        {
            return NotFound();
        }
        return View(planting); 
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var planting = _context.Plantings.Find(id);
        if (planting == null)
        {
            return NotFound();
        }

        _context.Plantings.Remove(planting);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index)); 
    }
}