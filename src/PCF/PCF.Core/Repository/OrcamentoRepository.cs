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

        public async Task<bool> CheckIfExistsGeralByIdAsync(int usuarioId)
        {
            return await _dbContext.Orcamentos.AnyAsync(c => c.CategoriaId == null && c.UsuarioId == usuarioId);
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
                .OrderBy(o => o.Categoria.Descricao)
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
                            o.UsuarioId = @UsuarioId
                        ORDER BY C.Nome";

            var parameters = new { UsuarioId = usuarioId };

            var result = await connection.QueryAsync<OrcamentoResponseViewModel>(query, parameters);

            return result;
        }

        public async Task<decimal> CheckAmountAvailableAsync(int usuarioId, DateTime data)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var inicioMes = new DateTime(data.Year, data.Month, 1);
            var fimMes = new DateTime(inicioMes.Year, inicioMes.Month, DateTime.DaysInMonth(inicioMes.Year, inicioMes.Month), 23, 59, 59);

            var query = @"
                        SELECT 
                            COALESCE(SUM(CASE WHEN t.Tipo = 0 THEN t.Valor ELSE 0 END), 0) -
                            COALESCE(SUM(CASE WHEN t.Tipo = 1 THEN t.Valor ELSE 0 END), 0) AS OrcamentoDisponivel
                        FROM
                            Transacao t
                        WHERE
                            t.UsuarioId = @UsuarioId AND
                            t.DataLancamento BETWEEN @InicioMes AND @FimMes";

            var parameters = new
            {
                UsuarioId = usuarioId,
                InicioMes = inicioMes,
                FimMes = fimMes
            };

            var result = await connection.QueryFirstOrDefaultAsync<decimal?>(query, parameters);
            return result ?? 0;

        }

        public async Task<decimal> CheckAmountUsedByCategoriaAsync(int usuarioId, DateTime data, int categoriaId)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var inicioMes = new DateTime(data.Year, data.Month, 1);
            var fimMes = new DateTime(inicioMes.Year, inicioMes.Month, DateTime.DaysInMonth(inicioMes.Year, inicioMes.Month), 23, 59, 59);

            var query = @"
                        SELECT 
                            COALESCE(SUM(CASE WHEN t.Tipo = 1 THEN t.Valor ELSE 0 END), 0) AS OrcamentoDisponivelCategoria
                        FROM
                            Transacao t
                        WHERE
                            t.UsuarioId = @UsuarioId AND
                            t.CategoriaId = @CategoriaId AND    
                            t.DataLancamento BETWEEN @InicioMes AND @FimMes";

            var parameters = new
            {
                UsuarioId = usuarioId,
                InicioMes = inicioMes,
                FimMes = fimMes,
                CategoriaId = categoriaId
            };

            var result = await connection.QueryFirstOrDefaultAsync<decimal?>(query, parameters);
            return result ?? 0;

        }
    }
}