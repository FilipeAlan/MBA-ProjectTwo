using Dapper;
using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Interface;

namespace PCF.Core.Repository
{
    public class TransacaoRepository(PCFDBContext dbContext) : Repository<Transacao>(dbContext), ITransacaoRepository
    {
        private readonly PCFDBContext _pCFDBContext = dbContext;

        public async Task<IEnumerable<Transacao>> GetAllAsync(int usuarioId)
        {
            return await _pCFDBContext.Transacoes
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetAllByCategoriaAsync(int usuarioId, int categoriaId)
        {
            return await _pCFDBContext.Transacoes
                .Where(t => t.UsuarioId == usuarioId && t.CategoriaId == categoriaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetAllByPeriodoAsync(int usuarioId, DateTime dataInicio, DateTime? dataFin)
        {
            if (!dataFin.HasValue)
            {
                return await _pCFDBContext.Transacoes
                    .Where(t => t.UsuarioId == usuarioId && t.DataLancamento.Date == dataInicio.Date)
                    .ToListAsync();
            }
            var result = await _pCFDBContext.Transacoes
                .Where(t => t.UsuarioId == usuarioId && t.DataLancamento.Date >= dataInicio.Date && t.DataLancamento <= dataFin.Value.Date)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Transacao>> GetAllByTipoAsync(int usuarioId, TipoEnum tipo)
        {
            return await _pCFDBContext.Transacoes
                .Where(t => t.UsuarioId == usuarioId && t.Tipo == tipo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetAllByTipoTransacaoAsync(TipoEnum tipo, int usuarioId)
        {
            return await _pCFDBContext.Transacoes
                .Where(t => t.UsuarioId == usuarioId && t.Tipo == tipo)
                .ToListAsync();
        }

        public async Task<Transacao?> GetByIdAsync(int id, int usuarioId)
        {
            return await _pCFDBContext.Transacoes
                .Include(t => t.Categoria)
                .SingleOrDefaultAsync(t => t.UsuarioId == usuarioId && t.Id == id);
        }

        public async Task<decimal> CheckTotalBudgetCurrentMonthAsync(int usuarioId, DateTime data)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var inicioMes = new DateTime(data.Year, data.Month, 1);
            var fimMes = new DateTime(inicioMes.Year, inicioMes.Month, DateTime.DaysInMonth(inicioMes.Year, inicioMes.Month), 23, 59, 59);

            var query = @"
                        SELECT 
                            COALESCE(SUM(CASE WHEN t.Tipo = 0 THEN t.Valor ELSE 0 END), 0)
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

        public async Task<decimal> CheckAmountUsedByCategoriaCurrentMonthAsync(int usuarioId, DateTime data, int categoriaId)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var inicioMes = new DateTime(data.Year, data.Month, 1);
            var fimMes = new DateTime(inicioMes.Year, inicioMes.Month, DateTime.DaysInMonth(inicioMes.Year, inicioMes.Month), 23, 59, 59);

            var query = @"
                        SELECT 
                            COALESCE(SUM(t.Valor), 0) AS OrcamentoDisponivelCategoria
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

        public async Task<decimal> CheckAmountUsedCurrentMonthAsync(int usuarioId, DateTime data)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            var inicioMes = new DateTime(data.Year, data.Month, 1);
            var fimMes = new DateTime(inicioMes.Year, inicioMes.Month, DateTime.DaysInMonth(inicioMes.Year, inicioMes.Month), 23, 59, 59);

            var query = @"
                        SELECT 
                            COALESCE(SUM(CASE WHEN t.Tipo = 1 THEN t.Valor ELSE 0 END), 0) AS OrcamentoTotalUtilizado
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
    }
}