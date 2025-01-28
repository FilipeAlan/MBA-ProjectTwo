using PCF.Core.Entities;

namespace PCF.Core.Interface
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetAllAsync(int usuarioId);

        Task<Categoria?> GetByIdAsync(int id, int usuarioId);

        Task<bool> CheckIfExistsByNomeAsync(int currentId, string nome, int usuarioId);
    }
}