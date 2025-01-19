using System.ComponentModel.DataAnnotations;

namespace PCF.Shared.Dtos
{
    public class CategoriaRequestViewModel
    {
        [Required]
        [MaxLength(250)]
        public required string Nome { get; set; }

        [MaxLength(500)]
        public string? Descricao { get; set; }
    }
}