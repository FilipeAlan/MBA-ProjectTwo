using PCF.Core.Dtos;
using PCF.Core.Entities;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class OrcamentoService(IAppIdentityUser appIdentityUser, IOrcamentoRepository repository, ICategoriaRepository categotiaRepository) : IOrcamentoService
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

            var result = Result.Ok()
                .WithSuccess("Orçamento atualizado com sucesso!");

            return result;
        }

        public async Task<Result<int>> AddAsync(Orcamento orcamento)
        {
            ArgumentNullException.ThrowIfNull(orcamento);

            if (orcamento.CategoriaId != null)
            {
                var categoria = await categotiaRepository.GetByIdAsync((int)orcamento.CategoriaId, appIdentityUser.GetUserId());

                if (categoria == null)
                {
                    categoria = await categotiaRepository.GetGeralByIdAsync((int)orcamento.CategoriaId);
                }

                if (categoria is null)
                {
                    return Result.Fail<int>("Categoria informada é inválida.");
                }

                if (await repository.CheckIfExistsByIdAsync(orcamento.CategoriaId.Value, appIdentityUser.GetUserId()))
                {
                    return Result.Fail<int>("Já existe um orçamento para essa categoria lançado");
                }
            }
            else
            {
                if (await repository.CheckIfExistsGeralByIdAsync(appIdentityUser.GetUserId()))
                {
                    return Result.Fail<int>("Já existe um orçamento geral lançado");
                }
            }

            orcamento.ValorLimite = orcamento.ValorLimite;
            orcamento.UsuarioId = appIdentityUser.GetUserId();
            orcamento.CategoriaId = orcamento.CategoriaId;

            await repository.CreateAsync(orcamento);
            return Result.Ok(orcamento.Id);
        }

        public async Task<IEnumerable<OrcamentoResponse>> GetAllWithDescriptionAsync()
        {
            return await repository.GetOrcamentoWithCategoriaAsync(appIdentityUser.GetUserId());
        }
    }
}