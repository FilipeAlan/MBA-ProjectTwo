using PCF.Core.Enumerables;

namespace PCF.Core.Dtos
{
    public record DashboardTransacoesPorCategoriaRequest(DateOnly Periodo, TipoEnum Tipo);
}