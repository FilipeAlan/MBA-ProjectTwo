using PCF.Data.Entities.Base;

namespace PCF.Data.Entities
{
    public class Categoria : Entity
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public bool Padrao { get; set; }
        public Usuario? Usuario { get; set; }

        public IEnumerable<Orcamento> Orcamentos { get; set; } = [];

        public IEnumerable<Transacao> Transacoes { get; set; } = [];
    }
}