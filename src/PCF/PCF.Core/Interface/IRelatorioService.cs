using PCF.Core.Entities;
using PCF.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCF.Core.Interface
{
    public interface IRelatorioService
    {
        Task<IEnumerable<RelatorioOrcamentoResponse>> GetOrcamentoRealizado(DateTime dataIncial, DateTime dataFinal);
    }
}
