using PCF.Core.Enumerables;
using System.ComponentModel.DataAnnotations;

namespace PCF.Core.Dtos
{
    public class TransacaoRequest
    {

        public required decimal Valor { get; set; }

        public int UsuarioId { get; set; }

        public int? CategoriaId { get; set; }

        public required string Descricao { get; set; }

        public required DateTime DataLancamento { get; set; }

        public TipoEnum Tipo { get; set; }

    }
}
