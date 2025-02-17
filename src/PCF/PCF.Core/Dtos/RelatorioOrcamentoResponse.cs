namespace PCF.Core.Dtos
{
    public class RelatorioOrcamentoResponse
    {
        public int TransacaoId { get; set; }
        public DateTime DataLancamento { get; set; }
        public string TipoLancamento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorLimite { get; set; }
        public string Descricao { get; set; }
        public int CategoriaId { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
    }
}
