using PCF.Core.Entities;

namespace PCF.Core.Interface
{
    public interface IOrcamentoRepository : IRepository<Orcamento>
    {
        Task<IEnumerable<Orcamento>> GetAllAsync(int usuarioId);

        Task<Orcamento?> GetByIdAsync(int id, int usuarioId);

        Task<bool> CheckIfExistsByIdAsync(int categoriaId, int usuarioId);
    }
}
