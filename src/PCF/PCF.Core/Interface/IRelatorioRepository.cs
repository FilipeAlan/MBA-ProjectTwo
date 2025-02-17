using PCF.Shared.Dtos;

namespace PCF.Core.Interface
{
    public interface IRelatorioRepository
    {
        Task<List<RelatorioOrcamentoResponse>> GetOrcamentoRealizadoAsync(DateTime dataInicial, DateTime dataFinal,int usuarioId);
    }
}