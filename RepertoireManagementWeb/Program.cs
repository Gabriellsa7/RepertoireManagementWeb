using RepertoireManagementWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));

// Adiciona serviços essenciais para autenticação via cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";   // Página de login
        options.LogoutPath = "/Logout"; // Página de logout
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Tempo do cookie (opcional)
    });

builder.Services.AddRazorPages();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// IMPORTANTE: ordem correta - primeiro autenticação, depois autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
