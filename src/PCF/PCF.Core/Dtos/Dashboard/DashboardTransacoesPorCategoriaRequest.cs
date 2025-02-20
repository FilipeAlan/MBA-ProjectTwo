using PCF.Core.Enumerables;

namespace PCF.Core.Dtos.Dashboard
{
    public record DashboardTransacoesPorCategoriaRequest(DateOnly Periodo, TipoEnum Tipo);
}