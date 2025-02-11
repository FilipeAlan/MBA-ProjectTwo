using System.ComponentModel.DataAnnotations;

namespace PCF.Core.Dtos
{
    public class OrcamentoRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O ValorLimite deve ser maior que zero.")]
        public required decimal ValorLimite { get; set; }

        public int? CategoriaId { get; set; }

    }
}
