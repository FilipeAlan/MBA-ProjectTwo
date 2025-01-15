using PCF.Data.Entities.Base;
using PCF.Data.Enumerables;

namespace PCF.Data.Entities
{
    public class Transacao : Entity
    {
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public required Categoria Categoria { get; set; }
        public required Usuario Usuario { get; set; }
        public DateTime DataLancamento { get; set; }
        public TipoEnum Tipo { get; set; }

        public void Validar()
        {
            if (Valor <= 0)
            {
                throw new Exception("Valor da transação deve ser maior que zero.");
            }
        }

    }
}