using PCF.Core.Entities.Base;

namespace PCF.Core.Entities
{
    public class Usuario : Entity
    {
        public required string Nome { get; set; }

        public virtual IEnumerable<Transacao> Transacoes { get; set; } = [];

        public virtual IEnumerable<Categoria> Categorias { get; set; } = [];

        public virtual IEnumerable<Orcamento> Orcamentos { get; set; } = [];
    }
}