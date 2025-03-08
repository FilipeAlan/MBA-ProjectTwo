using PCF.Core.Entities;

namespace PCF.Core.Interface
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetAllAsync();

        Task<Categoria?> GetByIdAsync(int id);

        Task<Result> DeleteAsync(int id);

        Task<Result> UpdateAsync(Categoria categoria);
        Task<bool> HasBudgetAssociationAsync(Categoria categoria);

        Task<Result<int>> AddAsync(Categoria categoria);
    }
}