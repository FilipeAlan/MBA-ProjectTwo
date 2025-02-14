using PCF.Core.Enumerables;

namespace PCF.Core.Dtos
{
    public class TransacaoRequest
    {
        public int? Id { get; set; }
        public required decimal Valor { get; set; }

        public int UsuarioId { get; set; }

        public int? CategoriaId { get; set; }

        public required string Descricao { get; set; }

        public required DateTime DataLancamento { get; set; }

        public TipoEnum Tipo { get; set; }

    }
}
