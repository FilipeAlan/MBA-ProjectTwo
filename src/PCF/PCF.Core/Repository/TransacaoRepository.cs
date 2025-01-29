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
            return await _pCFDBContext.Transacoes.Where(t => t.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetAllByCategoriaAsync(int usuarioId, int categoriaId)
        {
            return await _pCFDBContext.Transacoes.Where(t => t.UsuarioId == usuarioId && t.CategoriaId == categoriaId).ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetAllByPeriodoAsync(int usuarioId, DateTime dataInicio, DateTime? dataFin)
        {
            if (!dataFin.HasValue)
            {
                return await _pCFDBContext.Transacoes.Where(t => t.UsuarioId == usuarioId && t.DataLancamento.Date == dataInicio.Date).ToListAsync();
            }
            var result = await _pCFDBContext.Transacoes.Where(t => t.UsuarioId == usuarioId && t.DataLancamento.Date >= dataInicio.Date && t.DataLancamento <= dataFin.Value.Date).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Transacao>> GetAllByTipoAsync(int usuarioId, TipoEnum tipo)
        {
            return await _pCFDBContext.Transacoes.Where(t => t.UsuarioId == usuarioId && t.Tipo == tipo).ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetAllByTipoTransacaoAsync(TipoEnum tipo, int usuarioId)
        {
            return await _pCFDBContext.Transacoes.Where(t => t.UsuarioId == usuarioId && t.Tipo == tipo).ToListAsync();
        }

        public async Task<Transacao?> GetByIdAsync(int id, int usuarioId)
        {
            return await _pCFDBContext.Transacoes.SingleOrDefaultAsync(t => t.UsuarioId == usuarioId && t.Id == id);
        }
    }
}