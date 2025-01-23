using PCF.Core.Entities;
using PCF.Core.Interface;
using PCF.Shared.Dtos;

namespace PCF.Core.Services
{
    public class OrcamentoService(IAppIdentityUser appIdentityUser, IOrcamentoRepository repository) : IOrcamentoService
    {
        public async Task<IEnumerable<Orcamento>> GetAllAsync()
        {
            return await repository.GetAllAsync(appIdentityUser.GetUserId());
        }

        public async Task<Orcamento?> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id, appIdentityUser.GetUserId());
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var orcamento = await GetByIdAsync(id);

            if (orcamento is null)
            {
                return Result.Fail("Orçamento inexistente");
            }

            await repository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Orcamento orcamento)
        {
            ArgumentNullException.ThrowIfNull(orcamento);

            var orcamentoExistente = await GetByIdAsync(orcamento.Id);

            if (orcamentoExistente is null)
            {
                return Result.Fail("Orçamento inexistente");
            }

            orcamentoExistente.ValorLimite = orcamento.ValorLimite;

            await repository.UpdateAsync(orcamentoExistente);

            return Result.Ok();
        }

        public async Task<Result<int>> AddAsync(Orcamento orcamento)
        {
            ArgumentNullException.ThrowIfNull(orcamento);

            decimal orcamentoDisponivel = 0;

            if (orcamento.CategoriaId != null)
            {
                orcamentoDisponivel = await repository.CheckAmountAvailableByCategoriaAsync(appIdentityUser.GetUserId(), DateTime.Now, orcamento.CategoriaId.Value);
            }
            else
            {
                orcamentoDisponivel = await repository.CheckAmountAvailableAsync(appIdentityUser.GetUserId(), DateTime.Now);
            }

            if (orcamentoDisponivel < orcamento.ValorLimite)
            {
                return Result.Fail<int>("Valor do orçamento maior que o disponível");
            }

            orcamento.ValorLimite = orcamento.ValorLimite;
            orcamento.UsuarioId = appIdentityUser.GetUserId();
            orcamento.CategoriaId = orcamento.CategoriaId;

            await repository.CreateAsync(orcamento);
            return Result.Ok(orcamento.Id);
        }

        public async Task<IEnumerable<OrcamentoResponseViewModel>> GetAllWithDescriptionAsync()
        {
            return await repository.GetOrcamentoWithCategoriaAsync(appIdentityUser.GetUserId());
        }
    }
}