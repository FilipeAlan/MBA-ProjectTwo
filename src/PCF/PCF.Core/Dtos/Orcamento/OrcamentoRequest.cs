using System.ComponentModel.DataAnnotations;

namespace PCF.Core.Dtos.Orcamento
{
    public class OrcamentoRequest
    {
        [Required]
        public required decimal ValorLimite { get; set; }

        public int? CategoriaId { get; set; }

    }
}
