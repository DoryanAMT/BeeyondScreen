using BeeyondScreen.Data;
using Microsoft.EntityFrameworkCore;
using BeeyondScreen.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//  INCLIMOS LA SESSION Y CACHE A NUESTRO PROYECTO
builder.Services.AddSession();

string connectionString =
    builder.Configuration.GetConnectionString("SqlCine");

builder.Services.AddTransient<RepositoryCine>();

builder.Services.AddDbContext<CineContext>
    (options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Peliculas}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
