using PCF.Core.Entities.Base;

namespace PCF.Core.Entities
{
    public class Usuario : Entity
    {
        public required string Nome { get; set; }

        public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

        public virtual ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();

        public virtual ICollection<Orcamento> Orcamentos { get; set; } = new List<Orcamento>();

    }
}