using PCF.Core.Entities.Base;
using PCF.Core.Enumerables;

namespace PCF.Core.Entities
{
    public class Transacao : Entity
    {
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public int CategoriaId { get; set; }
        public virtual required Categoria Categoria { get; set; }
        public int UsuarioId { get; set; }
        public virtual required Usuario Usuario { get; set; }
        public DateTime DataLancamento { get; set; }
        public TipoEnum Tipo { get; set; }

        //TODO - Avaliar se é necessário criar um validador separado.
        public Boolean Validar()
        {
            if (Valor <= 0)
            {
                return false;
            }
            return true;
        }

    }
}