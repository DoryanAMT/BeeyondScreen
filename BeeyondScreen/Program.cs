using BeeyondScreen.Data;
using Microsoft.EntityFrameworkCore;
using BeeyondScreen.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//  INCLIMOS LA SESSION Y CACHE A NUESTRO PROYECTO
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultSignInScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie();
builder.Services
    .AddControllersWithViews(options => options.EnableEndpointRouting = false);
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

app.UseStaticFiles();

app.UseHttpsRedirection();
//app.UseRouting();

app.UseAuthorization();

//app.MapStaticAssets();

app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
      name: "Default",
      template: "{controller=Peliculas}/{action=Index}/{id?}"
    );
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Peliculas}/{action=Index}/{id?}")
//    .WithStaticAssets();


app.Run();
