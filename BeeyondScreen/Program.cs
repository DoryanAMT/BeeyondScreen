using BeeyondScreen.Data;
using BeeyondScreen.Repositories;
using Microsoft.EntityFrameworkCore;
using MvcBeeyondScreen.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//  INCLIMOS LA SESSION Y CACHE A NUESTRO PROYECTO
builder.Services.AddSession();

string connectionString =
    builder.Configuration.GetConnectionString("SqlCine");
builder.Services.AddTransient<RepositoryPelicula>();
builder.Services.AddTransient<RepositoryHorarioPelicula>();
builder.Services.AddTransient<RepositoryUsuario>();
builder.Services.AddTransient<RepositoryBoletos>();
builder.Services.AddTransient<RepositoryAsientos>();
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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
