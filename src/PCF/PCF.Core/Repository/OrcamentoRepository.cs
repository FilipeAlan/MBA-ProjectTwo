using Dapper;
using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Dtos;
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
        }

        public async Task<bool> CheckIfExistsGeralByIdAsync(int usuarioId)
        {
            return await _dbContext.Orcamentos.AnyAsync(c => c.CategoriaId == null && c.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Orcamento>> GetAllAsync(int usuarioId)
        {
            return await _dbContext.Orcamentos
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Orcamento?> GetByIdAsync(int id, int usuarioId)
        {
            return await _dbContext.Orcamentos
                .Include(c => c.Categoria)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);
        }

        public async Task<Orcamento?> GetByCategoriaAsync(int categoria, int usuarioId)
        {
            return await _dbContext.Orcamentos
                .Include(c => c.Categoria)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.CategoriaId == categoria && c.UsuarioId == usuarioId);
        }

        public async Task<Orcamento?> GetGeralAsync(int usuarioId)
        {
            return await _dbContext.Orcamentos
                .Include(c => c.Categoria)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.CategoriaId == null && c.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<OrcamentoResponse>> GetOrcamentoWithCategoriaAsync(int? usuarioId)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var query = @"
                        SELECT 
                            o.Id AS OrcamentoId, 
                            o.ValorLimite, 
                            o.UsuarioId,
                            o.CategoriaId,
                            c.Nome AS NomeCategoria,
                            u.Nome AS NomeUsuario
                        FROM 
                            Orcamento o
                        LEFT JOIN 
                            Categoria c ON o.CategoriaId = c.Id
                        LEFT JOIN
                            Usuario u ON o.UsuarioId = u.Id
                        WHERE 
                            o.UsuarioId = @UsuarioId
                        ORDER BY C.Nome";

            var parameters = new { UsuarioId = usuarioId };

            var result = await connection.QueryAsync<OrcamentoResponse>(query, parameters);

            return result;
        }
    }
}