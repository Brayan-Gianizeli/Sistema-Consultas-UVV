using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Senha { get; set; }

        public DateTime DataCadastro { get; set; }

        public List<Consulta> Consultas { get; set; } = new();
    }
}