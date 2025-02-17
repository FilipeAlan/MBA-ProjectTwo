namespace PCF.Core.Dtos
{
    public class OrcamentoResponse
    {
        public decimal ValorLimite { get; set; }
        public int OrcamentoId { get; set; }
        public int UsuarioId { get; set; }
        public string? NomeUsuario { get; set; }
        public int? CategoriaId { get; set; }
        public string? NomeCategoria { get; set; }
    }
}