using PCF.Core.Dtos.Relatorio;
using PCF.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCF.Core.Interface
{
    public interface IRelatorioService
    {
        Task<IEnumerable<RelatorioGastoPorCategoriaResponse>> GetGastoPorCategoria(DateTime dataIncial, DateTime dataFinal);
        Task<IEnumerable<RelatorioOrcamentoResponse>> GetOrcamentoRealizado(DateTime dataIncial, DateTime dataFinal);
    }
}
