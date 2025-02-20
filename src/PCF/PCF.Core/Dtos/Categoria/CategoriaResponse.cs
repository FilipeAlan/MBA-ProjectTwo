namespace PCF.Core.Dtos.Categoria
{
    public class CategoriaResponse
    {
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? ModificadoEm { get; set; }
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public bool Padrao { get; set; }
    }
}