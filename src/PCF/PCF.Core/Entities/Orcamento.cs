using PCF.Core.Entities.Base;

namespace PCF.Core.Entities
{
    public class Orcamento : Entity
    {
        public decimal ValorLimite { get; set; }
        public int UsuarioId { get; set; }
        public virtual required Usuario Usuario { get; set; }
        public int? CategoriaId { get; set; }
        public virtual required Categoria? Categoria { get; set; }
    }
}