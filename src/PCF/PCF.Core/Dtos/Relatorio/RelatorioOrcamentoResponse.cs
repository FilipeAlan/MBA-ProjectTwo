namespace PCF.Core.Dtos.Relatorio
{
    public class RelatorioOrcamentoResponse
    {
        public int TransacaoId { get; set; }
        public DateTime DataLancamento { get; set; }
        public required string TipoLancamento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorLimite { get; set; }
        public string? Descricao { get; set; }
        public int CategoriaId { get; set; }
        public required string Categoria { get; set; }
        public required string Tipo { get; set; }
        public int UsuarioId { get; set; }
        public required string Usuario { get; set; }
    }
}
