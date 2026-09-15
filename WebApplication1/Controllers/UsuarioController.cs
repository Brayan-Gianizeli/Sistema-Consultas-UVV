using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Abre a tela de cadastro
        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        // Recebe os dados do formulário
        [HttpPost]
        public async Task<IActionResult> Cadastro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            usuario.DataCadastro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }
    }
}