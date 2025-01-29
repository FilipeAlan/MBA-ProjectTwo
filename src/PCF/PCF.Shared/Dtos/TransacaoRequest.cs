using System.ComponentModel.DataAnnotations;

namespace PCF.Shared.Dtos
{
    public class TransacaoRequest
    {
        
        public required decimal Valor { get; set; }

        public int UsuarioId { get; set; }

        public int? CategoriaId { get; set; }

        public required string Descricao { get; set; }

        public required DateTime DataLancamento { get; set; }

        public TipoEnumDto Tipo { get; set; }

    }
}
