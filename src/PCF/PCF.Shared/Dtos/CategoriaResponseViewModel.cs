namespace PCF.Shared.Dtos
{
    public class CategoriaResponseViewModel
    {
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? ModificadoEm { get; set; }
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public bool Padrao { get; set; }
    }
}