using PCF.Data.Entities.Base;

namespace PCF.Data.Entities
{
    public class Usuario : Entity
    {
        public required string Nome { get; set; }

        public IEnumerable<OrcamentoGeral> OrcamentosGerais { get; set; } = [];

        public IEnumerable<Transacao> Transacoes { get; set; } = [];

        public IEnumerable<Categoria> Categorias { get; set; } = [];
    }
}