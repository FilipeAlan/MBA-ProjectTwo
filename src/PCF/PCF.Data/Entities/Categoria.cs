namespace PCF.Data.Entities
{
    public class Categoria:Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Padrao { get; set; }
        public Usuario Usuario { get; set; }
        public IEnumerable<Orcamento> Orcamento { get; set; }
    }
}
