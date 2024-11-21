using FazendaUrbanaSolNascente.Context;
using FazendaUrbanaSolNascente.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FazendaUrbanaSolNascente.Controllers;

public class ReportsController : Controller
{
    private readonly SolNascerDbContext _context;

    public ReportsController(SolNascerDbContext context)
    {
        _context = context;
    }

    public IActionResult SalesReport()
    {
        var sales = _context.Vendas.Include(s => s.Customer).Include(s => s.SaleItems).ThenInclude(si => si.Product).ToList();
        var viewModel = new SalesReportViewModel
        {
            Sales = sales,
            TotalVendido = sales.SelectMany(s => s.SaleItems).Sum(si => si.Product.Price * si.Quantity)
        };
        return View(viewModel);
    }

    public async Task<IActionResult> DownloadSalesReport()
    {
        var sales = await _context.Vendas
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
            .ToListAsync();

        var totalVendido = sales.SelectMany(s => s.SaleItems)
                                .Sum(si => si.Product.Price * si.Quantity);

        using (var ms = new MemoryStream())
        {
            // Cria o documento PDF com o MemoryStream como saída
            var document = new Document();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            // Define as fontes para o título, cabeçalho, células e total
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
            var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.RED);

            // Adiciona o título do relatório
            var titleParagraph = new Paragraph("Relatório de Vendas", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            document.Add(titleParagraph);
            document.Add(new Paragraph(" ")); // Espaço após o título

            // Cria a tabela com as colunas devidas
            var table = new PdfPTable(5) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 2, 2, 2, 1, 2 });

            // Adiciona o cabeçalho da tabela
            table.AddCell(new PdfPCell(new Phrase("Data da Venda", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Cliente", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Produto", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Quantidade", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Valor Total", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Preenche a tabela com os dados de vendas e produtos
            foreach (var sale in sales)
            {
                foreach (var item in sale.SaleItems)
                {
                    table.AddCell(new PdfPCell(new Phrase(sale.SaleDate.ToShortDateString(), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(sale.Customer?.Name ?? "Cliente Não Informado", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Product?.Name ?? "Produto Não Informado", cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), cellFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase((item.Quantity * item.Product.Price).ToString("C"), cellFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                }
            }

            document.Add(table);
            document.Add(new Paragraph(" ")); // Espaço antes do total

            // Adiciona o total vendido em vermelho
            var totalParagraph = new Paragraph($"Total Vendido: {totalVendido:C}", totalFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            document.Add(totalParagraph);

            document.Close();

            // Retorna o PDF gerado como um arquivo de download
            return File(ms.ToArray(), "application/pdf", "RelatorioVendas.pdf");
        }
    }


    public async Task<IActionResult> DownloadProductReport()
    {
        var products = await _context.Produtos.ToListAsync();

        var totalEmEstoque = products.Sum(p => p.QtdStorage);
        var valorTotalProdutos = products.Sum(p => p.Price * p.QtdStorage);

        using (var ms = new MemoryStream())
        {
            var document = new Document();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
            var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.RED);

            var titleParagraph = new Paragraph("Relatório de Produtos", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            document.Add(titleParagraph);
            document.Add(new Paragraph(" ")); 

            var table = new PdfPTable(3) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 3, 1, 1 }); 

            table.AddCell(new PdfPCell(new Phrase("Nome", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Preço", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Quantidade em Estoque", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

            foreach (var product in products)
            {
                table.AddCell(new PdfPCell(new Phrase(product.Name, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(product.Price.ToString("C"), cellFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(product.QtdStorage.ToString(), cellFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
            }

            document.Add(table);
            document.Add(new Paragraph(" ")); 

            var totalParagraph = new Paragraph($"Total em Estoque: {totalEmEstoque}", totalFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            document.Add(totalParagraph);

            var valorTotalParagraph = new Paragraph($"Valor Total dos Produtos: {valorTotalProdutos:C}", totalFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            document.Add(valorTotalParagraph);

            document.Close();

            return File(ms.ToArray(), "application/pdf", "RelatorioProdutos.pdf");
        }
    }

    public IActionResult PurchaseReport()
    {
        var products = _context.Produtos.ToList();
        return View(products);
    }
}
