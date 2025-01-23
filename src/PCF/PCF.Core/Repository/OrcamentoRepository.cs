using Dapper;
using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Entities;
using PCF.Core.Interface;
using PCF.Shared.Dtos;

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

        public async Task<IEnumerable<OrcamentoResponseViewModel>> GetOrcamentoWithCategoriaAsync(int? usuarioId)
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
                            Orcamentos o
                        LEFT JOIN 
                            Categorias c ON o.CategoriaId = c.Id
                        LEFT JOIN
                            Usuarios u ON o.UsuarioId = u.Id
                        WHERE 
                            (o.UsuarioId = @UsuarioId OR o.UsuarioId IS NULL)";

            var parameters = new { UsuarioId = usuarioId };

            var result = await connection.QueryAsync<OrcamentoResponseViewModel>(query, parameters);

            return result;
        }
    }
}