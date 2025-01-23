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
                            Orcamento o
                        LEFT JOIN 
                            Categoria c ON o.CategoriaId = c.Id
                        LEFT JOIN
                            Usuario u ON o.UsuarioId = u.Id
                        WHERE 
                            (o.UsuarioId = @UsuarioId OR o.UsuarioId IS NULL)";

            var parameters = new { UsuarioId = usuarioId };

            var result = await connection.QueryAsync<OrcamentoResponseViewModel>(query, parameters);

            return result;
        }

        public async Task<decimal> CheckAmountAvailableAsync(int usuarioId, DateTime data)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var inicioMes = new DateTime(data.Year, data.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var query = @"
                        SELECT 
                            COALESCE(SUM(CASE WHEN t.Tipo = 0 THEN t.Valor ELSE 0 END), 0) -
                            COALESCE(SUM(CASE WHEN t.Tipo = 1 THEN t.Valor ELSE 0 END), 0) AS OrcamentoDisponivel
                        FROM
                            Transacao t
                        WHERE
                            t.UsuarioId = @UsuarioId AND
                            t.Data BETWEEN @InicioMes AND @FimMes";

            var parameters = new
            {
                UsuarioId = usuarioId,
                InicioMes = inicioMes,
                FimMes = fimMes
            };

            return await connection.QueryFirstOrDefault(query, parameters);

        }

        public async Task<decimal> CheckAmountAvailableByCategoriaAsync(int usuarioId, DateTime data, int categoriaId)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var inicioMes = new DateTime(data.Year, data.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var query = @"
                        SELECT 
                            COALESCE(SUM(CASE WHEN t.Tipo = 0 THEN t.Valor ELSE 0 END), 0) -
                            COALESCE(SUM(CASE WHEN t.Tipo = 1 THEN t.Valor ELSE 0 END), 0) AS OrcamentoDisponivelCategoria
                        FROM
                            Transacao t
                        WHERE
                            t.UsuarioId = @UsuarioId AND
                            t.CategoriaId = @CategoriaId AND    
                            t.Data BETWEEN @InicioMes AND @FimMes";

            var parameters = new
            {
                UsuarioId = usuarioId,
                InicioMes = inicioMes,
                FimMes = fimMes,
                CategoriaId = categoriaId
            };

            return await connection.QueryFirstOrDefault(query, parameters);

        }
    }
}