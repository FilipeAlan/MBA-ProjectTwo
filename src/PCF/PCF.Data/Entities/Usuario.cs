namespace PCF.Data.Entities
{
    public class Usuario:Entity
    {
        public IEnumerable<OrcamentoGeral> OrcamentosGerais { get; set; }
    }
}
