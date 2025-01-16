using PCF.Core.Entities.Base;

namespace PCF.Core.Entities
{
    public class Categoria : Entity
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public bool Padrao { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public IEnumerable<Orcamento> Orcamentos { get; set; } = [];

        public IEnumerable<Transacao> Transacoes { get; set; } = [];
    }
}