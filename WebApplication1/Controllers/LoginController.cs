using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.Senha == senha);

            if (usuario == null)
            {
                ViewBag.Erro = "E-mail ou senha incorretos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var identidade = new ClaimsIdentity(
                claims,
                "LoginCookie");

            var principal = new ClaimsPrincipal(identidade);

            await HttpContext.SignInAsync(
                "LoginCookie",
                principal);

            return RedirectToAction("Index", "Consulta");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("LoginCookie");

            return RedirectToAction("Index", "Home");
        }
    }
}