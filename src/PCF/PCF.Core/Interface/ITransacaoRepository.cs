using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Shared.Dtos;

namespace PCF.Core.Interface
{
    public interface ITransacaoRepository : IRepository<Transacao>
    {
        Task<IEnumerable<Transacao>> GetAllAsync(int usuarioId);

        Task<IEnumerable<Transacao>> GetAllByCategoriaAsync(int usuarioId, int categoriaId);

        Task<IEnumerable<Transacao>> GetAllByPeriodoAsync(int usuarioId, DateTime dataInicio, DateTime? dataFin);

        Task<IEnumerable<Transacao>> GetAllByTipoAsync(int usuarioId, TipoEnum tipo);

        Task<Transacao?> GetByIdAsync(int id, int usuarioId);

        Task<IEnumerable<Transacao>> GetAllByTipoTransacaoAsync(TipoEnum tipo, int usuarioId);

    }
}