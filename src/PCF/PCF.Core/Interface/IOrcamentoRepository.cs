using PCF.Core.Entities;
using PCF.Shared.Dtos;

namespace PCF.Core.Interface
{
    public interface IOrcamentoRepository : IRepository<Orcamento>
    {
        Task<IEnumerable<Orcamento>> GetAllAsync(int usuarioId);

        Task<Orcamento?> GetByIdAsync(int id, int usuarioId);

        Task<bool> CheckIfExistsByIdAsync(int categoriaId, int usuarioId);

        Task<IEnumerable<OrcamentoResponseViewModel>> GetOrcamentoWithCategoriaAsync(int? usuarioId);

        Task<decimal> CheckAmountAvailableAsync(int usuarioId, DateTime data);

        Task<decimal> CheckAmountAvailableByCategoriaAsync(int usuarioId, DateTime data, int categoriaId);
    }
}
