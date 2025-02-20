using PCF.Core.Enumerables;

namespace PCF.Core.Dtos.Transacao
{
    public class TransacaoResponse
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataLancamento { get; set; }
        public TipoEnum Tipo { get; set; }
    }
}