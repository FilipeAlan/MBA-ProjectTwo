using PCF.Core.Entities;

namespace PCF.Core.Interface
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetAllAsync(int usuarioId);

        Task<Categoria?> GetByIdAsync(int id, int usuarioId);
        Task<Categoria?> GetGeralByIdAsync(int id);
        Task<bool> HasBudgetAssociationAsync(Categoria categoria);
        Task<bool> CheckIfExistsByNomeAsync(int currentId, string nome, int usuarioId);
    }
}