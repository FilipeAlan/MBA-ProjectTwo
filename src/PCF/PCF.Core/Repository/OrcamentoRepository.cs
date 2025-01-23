using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Entities;
using PCF.Core.Interface;

namespace PCF.Core.Repository
{
    public class OrcamentoRepository : Repository<Orcamento>, IOrcamentoRepository
    {
        public OrcamentoRepository(PCFDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckIfExistsByIdAsync(int categoriaId, int usuarioId)
        {
            return await _dbContext.Orcamentos.AnyAsync(c => c.CategoriaId == categoriaId && c.UsuarioId == usuarioId);
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Orcamento>> GetAllAsync(int usuarioId)
        {
            return await _dbContext.Orcamentos
                .Where(c => c.UsuarioId == usuarioId || c.UsuarioId == null)
                .ToListAsync();
        }

        public async Task<Orcamento?> GetByIdAsync(int id, int usuarioId)
        {
            return await _dbContext.Orcamentos
                .Include(c => c.Categoria)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);
        }
    }
}