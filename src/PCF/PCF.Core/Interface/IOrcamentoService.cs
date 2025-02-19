using PCF.Core.Dtos;
using PCF.Core.Dtos.Orcamento;
using PCF.Core.Entities;

namespace PCF.Core.Interface
{
    public interface IOrcamentoService
    {
        Task<IEnumerable<Orcamento>> GetAllAsync();

        Task<Orcamento?> GetByIdAsync(int id);

        Task<Result> DeleteAsync(int id);

        Task<Result<GlobalResult>> UpdateAsync(Orcamento orcamento);

        Task<Result<GlobalResult>> AddAsync(Orcamento orcamento);

        Task<IEnumerable<OrcamentoResponse>> GetAllWithDescriptionAsync();
    }
}