using FazendaUrbanaSolNascente.Context;
using FazendaUrbanaSolNascente.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FazendaUrbanaSolNascente.Controllers
{
    public class ProductController : Controller
    {
        private readonly SolNascerDbContext _context;

        public ProductController(SolNascerDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await ProcessarPlantiosPassados();

            return View(await _context.Produtos.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Produtos.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Produtos.Any(e => e.Id == product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Produtos.FindAsync(id);
            if (product != null)
            {
                _context.Produtos.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        private async Task ProcessarPlantiosPassados()
        {
            // Obtém os plantios com data anterior a hoje
            var plantiosPassados = await _context.Plantings
                .Where(p => p.HarvestDate.Date < DateTime.Now.Date)
                .ToListAsync();

            foreach (var plantio in plantiosPassados)
            {
                // Busca o produto associado ao plantio
                var produto = await _context.Produtos.FindAsync(plantio.ProductId);

                if (produto != null)
                {
                    // Adiciona a quantidade do plantio ao estoque do produto
                    produto.QtdStorage += plantio.Quantidade; // 'Quantity' é a quantidade que você quer adicionar ao estoque

                    // Atualiza o produto no banco de dados
                    _context.Produtos.Update(produto);
                }

                _context.Plantings.Remove(plantio);
            }

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();
        }
    }
}
