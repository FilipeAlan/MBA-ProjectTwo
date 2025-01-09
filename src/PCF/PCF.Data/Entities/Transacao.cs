
using PCF.Data.Enumerables;

namespace PCF.Data.Entities
{
    public class Transacao:Entity
    {
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public Categoria Categoria { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime DataLancamento { get; set; }
        public TipoEnum Tipo { get; set; }
    }
}
