using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticação
builder.Services.AddAuthentication("LoginCookie")
    .AddCookie("LoginCookie", options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
    });

// Autorização
builder.Services.AddAuthorization();

var app = builder.Build();

// Configuração do ambiente
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// IMPORTANTE: Authentication antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

// Rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();