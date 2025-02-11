using System.ComponentModel.DataAnnotations;

namespace PCF.Core.Dtos
{
    public class CategoriaRequest
    {
        [Required]
        [MaxLength(250)]
        public required string Nome { get; set; }

        [MaxLength(500)]
        public string? Descricao { get; set; }
    }
}