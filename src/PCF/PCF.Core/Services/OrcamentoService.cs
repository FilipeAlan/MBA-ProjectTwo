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

            decimal orcamentoUtilizadoCategoria = 0;
            decimal orcamentoGeral = await repository.CheckAmountAvailableAsync(appIdentityUser.GetUserId(), DateTime.Now);

            if (orcamentoExistente.CategoriaId != null)
            {
                orcamentoUtilizadoCategoria =
                    await repository.CheckAmountUsedByCategoriaAsync(appIdentityUser.GetUserId(), DateTime.Now,
                        orcamentoExistente.CategoriaId.Value);

                if (orcamentoUtilizadoCategoria > orcamento.ValorLimite)
                {
                    return Result.Fail(
                        "Valor do orçamento para a categoria informada maior que os lançamentos alocados");
                }

                if (orcamento.ValorLimite > orcamentoGeral)
                {
                    return Result.Fail(
                        "Valor do orçamento da categoria não pode ser maior que o total disponível");
                }
            }
            else
            {
                if (orcamentoGeral < orcamento.ValorLimite)
                {
                    return Result.Fail("Valor do orçamento maior que o disponível");
                }
            }

            orcamentoExistente.ValorLimite = orcamento.ValorLimite;

            await repository.UpdateAsync(orcamentoExistente);

            return Result.Ok();
        }

        public async Task<Result<int>> AddAsync(Orcamento orcamento)
        {
            ArgumentNullException.ThrowIfNull(orcamento);

            decimal orcamentoUtilizadoCategoria = 0;
            decimal orcamentoGeral = await repository.CheckAmountAvailableAsync(appIdentityUser.GetUserId(), DateTime.Now);

            if (orcamento.CategoriaId != null)
            {

                if (await repository.CheckIfExistsByIdAsync(orcamento.CategoriaId.Value, appIdentityUser.GetUserId()))
                {
                    return Result.Fail<int>("Já existe um orçamento para essa categoria lançado");
                }

                orcamentoUtilizadoCategoria =
                    await repository.CheckAmountUsedByCategoriaAsync(appIdentityUser.GetUserId(), DateTime.Now,
                        orcamento.CategoriaId.Value);

                if (orcamentoUtilizadoCategoria > orcamento.ValorLimite)
                {
                    return Result.Fail<int>(
                        "Valor do orçamento para a categoria informada maior que os lançamentos alocados");
                }

                if (orcamento.ValorLimite > orcamentoGeral)
                {
                    return Result.Fail<int>(
                        "Valor do orçamento da categoria não pode ser maior que o total disponível");
                }
            }
            else
            {
                if (await repository.CheckIfExistsGeralByIdAsync(appIdentityUser.GetUserId()))
                {
                    return Result.Fail<int>("Já existe um orçamento geral lançado");
                }

                if (orcamentoGeral < orcamento.ValorLimite)
                {
                    return Result.Fail<int>("Valor do orçamento maior que o disponível");
                }
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