using PCF.Data.Entities.Base;

namespace PCF.Data.Entities
{
    public class OrcamentoGeral : Entity
    {
        public decimal ValorLimite { get; set; }
        public required Usuario Usuario { get; set; }
    }
}