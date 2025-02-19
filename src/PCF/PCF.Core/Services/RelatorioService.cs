using PCF.Core.Dtos.Relatorio;
using PCF.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCF.Core.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IAppIdentityUser _appIdentityUser;
        private readonly IRelatorioRepository _relatorioRepository;

        public RelatorioService(IAppIdentityUser appIdentityUser, IRelatorioRepository relatorioRepository)
        {
            _appIdentityUser = appIdentityUser;
            _relatorioRepository = relatorioRepository;
        }

        public async Task<IEnumerable<RelatorioOrcamentoResponse>> GetOrcamentoRealizado(DateTime dataIncial, DateTime dataFinal)
        {
            var userId = _appIdentityUser.GetUserId();
            var result = await _relatorioRepository.GetOrcamentoRealizadoAsync(dataIncial, dataFinal,userId);
            return result;
        }
    }
}
