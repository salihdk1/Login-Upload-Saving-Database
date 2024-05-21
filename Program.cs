using LDap.Helpers;
using LDap.Data;
using LDap.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s�n� ekleyin
builder.Services.AddDbContext<DbClass>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

// Identity servisini ekleyin
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<DbClass>();

// Ldap ayarlar�n� yap�land�r�n ve LdapAuthenticationService'yi ekleyin
builder.Services.Configure<LdapSettings>(builder.Configuration.GetSection("LdapSettings"));
builder.Services.AddScoped<LdapAuthenticationService>();

// Controller ve Views'leri ekleyin
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Geli�tirme ortam�nda hata sayfas�n� kullan�n, aksi takdirde HSTS kullan�n
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication ve Authorization middleware'lerini ekleyin
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
