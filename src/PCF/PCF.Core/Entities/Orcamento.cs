using PCF.Core.Entities.Base;

namespace PCF.Core.Entities
{
    public class Orcamento : Entity
    {
        public decimal ValorLimite { get; set; }
        public int UsuarioId { get; set; }
        public virtual required Usuario Usuario { get; set; }
        public int? CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }

        public Boolean Validar()
        {
            if (ValorLimite <= 0)
            {
                return false;
            }
            return true;
        }
    }
}