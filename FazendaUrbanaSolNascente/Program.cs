using FazendaUrbanaSolNascente.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SolNascerDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();