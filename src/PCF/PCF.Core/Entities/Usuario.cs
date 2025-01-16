using PCF.Core.Entities.Base;

namespace PCF.Core.Entities
{
    public class Usuario : Entity
    {
        public required string Nome { get; set; }

        public IEnumerable<Transacao> Transacoes { get; set; } = [];

        public IEnumerable<Categoria> Categorias { get; set; } = [];
    }
}