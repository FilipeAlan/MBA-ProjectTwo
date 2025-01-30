namespace PCF.Shared.Dtos
{
    public class TransacaoResponse
    {
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataLancamento { get; set; }
        public TipoEnumDto Tipo { get; set; }
    }
}