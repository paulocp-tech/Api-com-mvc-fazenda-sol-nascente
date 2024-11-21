using FazendaUrbanaSolNascente.Models;
using Microsoft.EntityFrameworkCore;

namespace FazendaUrbanaSolNascente.Context;

public class SolNascerDbContext : DbContext
{
    public SolNascerDbContext(DbContextOptions<SolNascerDbContext> options) : base(options) { }

    public DbSet<Product> Produtos { get; set; }
    public DbSet<Sale> Vendas { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Planting> Plantings { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SaleItem>()
            .HasKey(si => new { si.SaleId, si.ProductId });

        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Sale)
            .WithMany(s => s.SaleItems)
            .HasForeignKey(si => si.SaleId);

        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Product)
            .WithMany(p => p.SaleItems)
            .HasForeignKey(si => si.ProductId);

        modelBuilder.Entity<Product>()
        .Property(p => p.Price)
        .HasPrecision(18, 2); // Define a precisão de 18 dígitos e 2 casas decimais

        modelBuilder.Entity<Sale>()
            .Property(s => s.TotalValue)
            .HasPrecision(18, 2); // Define a precisão de 18 dígitos e 2 casas decimais
    }
}