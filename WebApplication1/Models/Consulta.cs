using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Consulta
    {
        public int Id { get; set; }

        [Required]
        public string Especialidade { get; set; } = string.Empty;

        [Required]
        public DateTime DataHora { get; set; }

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}