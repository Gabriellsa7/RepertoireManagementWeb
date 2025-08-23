using RepertoireManagementWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using RepertoireManagementWeb.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));

// Add a essentials services to authentication by cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";   // Login Page
        options.LogoutPath = "/Logout"; // Logout Page
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Cookie Time
    });

builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.MapHub<ShowHub>("/showHub");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Important: correct order - first authentication, second authorization 
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();

app.Run();
