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

        Task<Orcamento?> GetByCategoriaAsync(int categoria, int usuarioId);

        Task<Orcamento?> GetGeralAsync(int usuarioId);
    }
}