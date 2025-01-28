using System.ComponentModel.DataAnnotations;

namespace PCF.Shared.Dtos
{
    public class OrcamentoRequestViewModel
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O ValorLimite deve ser maior que zero.")]
        public required decimal ValorLimite { get; set; }

        public int? CategoriaId { get; set; }

    }
}
