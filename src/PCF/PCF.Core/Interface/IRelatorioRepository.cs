using PCF.Core.Dtos.Relatorio;

namespace PCF.Core.Interface
{
    public interface IRelatorioRepository
    {
        Task<List<RelatorioGastoPorCategoriaResponse>> GetGastoPorCategoriaAsync(DateTime dataInicial, DateTime dataFinal, int usuarioId);
        Task<List<RelatorioOrcamentoResponse>> GetOrcamentoRealizadoAsync(DateTime dataInicial, DateTime dataFinal,int usuarioId);
    }
}