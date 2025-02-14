using PCF.Core.Dtos;
using PCF.Core.Entities;

namespace PCF.Core.Interface
{
    public interface IOrcamentoRepository : IRepository<Orcamento>
    {
        Task<IEnumerable<Orcamento>> GetAllAsync(int usuarioId);

        Task<Orcamento?> GetByIdAsync(int id, int usuarioId);

        Task<bool> CheckIfExistsByIdAsync(int categoriaId, int usuarioId);
        
        Task<bool> CheckIfExistsGeralByIdAsync(int usuarioId);

        Task<IEnumerable<OrcamentoResponse>> GetOrcamentoWithCategoriaAsync(int? usuarioId);

        Task<decimal> CheckTotalBudgetAsync(int usuarioId, DateTime data);

        Task<decimal> CheckAmountUsedByCategoriaAsync(int usuarioId, DateTime data, int categoriaId);
    }
}
