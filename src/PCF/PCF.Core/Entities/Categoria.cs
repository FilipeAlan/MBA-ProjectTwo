using PCF.Core.Entities.Base;

namespace PCF.Core.Entities
{
    public class Categoria : Entity
    {
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public bool Padrao { get; set; }
        public int? UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<Orcamento> Orcamentos { get; set; } = [];

        public virtual ICollection<Transacao> Transacoes { get; set; } = [];
    }
}