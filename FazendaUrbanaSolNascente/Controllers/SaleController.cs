using FazendaUrbanaSolNascente.Context;
using FazendaUrbanaSolNascente.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SaleController : Controller
{
    private readonly SolNascerDbContext _context;

    public SaleController(SolNascerDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        var viewModel = new CreateSaleViewModel
        {
            Produtos = _context.Produtos.ToList(),
            Customer = _context.Customers.ToList(), 
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Reports()
    {
        var sales = await _context.Vendas
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .ToListAsync();

        return View(sales);
    }

    [HttpPost]
    public IActionResult AddToCart(int selectedProductId, int quantity)
    {
        var product = _context.Produtos.Find(selectedProductId);
        if (product != null && quantity > 0 && quantity <= product.QtdStorage)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.Product.Id == selectedProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { Product = product, Quantity = quantity });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        return RedirectToAction("Create");
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        var cartItem = cart.FirstOrDefault(c => c.Product.Id == productId);

        if (cartItem != null)
        {
            cart.Remove(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        return RedirectToAction("Create");
    }

    // Método POST para processar a venda
    [HttpPost]
    public async Task<IActionResult> FinalizeSale(int customerId)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

        if (cart != null && cart.Any())
        {
            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                TotalValue = cart.Sum(item => item.Product.Price * item.Quantity),
                CustomerId = customerId // Associa o cliente selecionado à venda
            };

            _context.Vendas.Add(sale);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                var product = await _context.Produtos.FindAsync(item.Product.Id);
                if (product != null && product.QtdStorage >= item.Quantity)
                {
                    product.QtdStorage -= item.Quantity;

                    var saleItem = new SaleItem
                    {
                        SaleId = sale.Id,
                        ProductId = product.Id,
                        Quantity = item.Quantity
                    };
                    _context.SaleItems.Add(saleItem);
                }
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Create");
        }

        return RedirectToAction("Create");
    }
}