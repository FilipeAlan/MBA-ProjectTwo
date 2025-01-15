using PCF.Data.Entities.Base;

namespace PCF.Data.Entities
{
    public class Orcamento : Entity
    {
        public decimal ValorLimite { get; set; }
        public required Usuario Usuario { get; set; }
        public required Categoria Categoria { get; set; }
    }
}