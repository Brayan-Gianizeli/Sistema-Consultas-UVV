using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultaController(AppDbContext context)
        {
            _context = context;
        }

        // Lista as consultas do usuário logado
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == usuarioId)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return View(consultas);
        }

        // Abre a tela para cadastrar uma consulta
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        // Salva a nova consulta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Consulta consulta)
        {
            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            consulta.UsuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Abre a tela para editar
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // Salva as alterações
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Consulta consulta)
        {
            if (id != consulta.Id)
            {
                return NotFound();
            }

            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var consultaBanco = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consultaBanco == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            consultaBanco.Especialidade = consulta.Especialidade;
            consultaBanco.DataHora = consulta.DataHora;
            consultaBanco.Descricao = consulta.Descricao;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Confirmação de exclusão
        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // Exclui a consulta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consulta == null)
            {
                return NotFound();
            }

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}