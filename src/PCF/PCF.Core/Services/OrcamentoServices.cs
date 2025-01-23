using PCF.Core.Entities;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class OrcamentoServices(IAppIdentityUser appIdentityUser, IOrcamentoRepository repository) : IOrcamentoService
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

            //if (await repository.CheckIfExistsByIdAsync(orcamento.CategoriaId, appIdentityUser.GetUserId()))
            //{
            //    return Result.Fail<int>("Categoria já lançada para o usuário");
            //}

            orcamento.ValorLimite = orcamento.ValorLimite;
            orcamento.UsuarioId = appIdentityUser.GetUserId();
            

            await repository.CreateAsync(orcamento);
            return Result.Ok(orcamento.Id);
        }
    }
}