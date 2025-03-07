using FluentResults;
using PCF.Core.Dtos.Global;
using PCF.Core.Dtos.Orcamento;
using PCF.Core.Entities;
using PCF.Core.Globalization;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class OrcamentoService(IAppIdentityUser appIdentityUser, IOrcamentoRepository repository, ICategoriaRepository categotiaRepository) : IOrcamentoService
    {
        private string mensagemErro = string.Empty;
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

        public async Task<Result<GlobalResult>> UpdateAsync(Orcamento orcamento)
        {
            ArgumentNullException.ThrowIfNull(orcamento);

            var orcamentoExistente = await GetByIdAsync(orcamento.Id);

            if (orcamentoExistente is null)
            {
                return Result.Fail("Orçamento inexistente");
            }

            if (!orcamentoExistente.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }
            var isValorValido = await ValidaSomaOrcamentoAsync(orcamento.ValorLimite, orcamentoExistente.ValorLimite,orcamento.CategoriaId);
            if (!isValorValido)
            {
                return Result.Fail(mensagemErro);
            }

            orcamentoExistente.ValorLimite = orcamento.ValorLimite;

            await repository.UpdateAsync(orcamentoExistente);

            var result = Result.Ok()
                .WithSuccess("Orçamento atualizado com sucesso!");

            return Result.Ok(new GlobalResult(default, string.Join(", ", result.Errors.Select(e => e.Message))));
        }

        public async Task<Result<GlobalResult>> AddAsync(Orcamento orcamento)
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
                    return Result.Fail("Categoria informada é inválida.");
                }

                if (await repository.CheckIfExistsByIdAsync(orcamento.CategoriaId.Value, appIdentityUser.GetUserId()))
                {
                    return Result.Fail("Já existe um orçamento para essa categoria lançado");
                }
            }
            else
            {
                if (await repository.CheckIfExistsGeralByIdAsync(appIdentityUser.GetUserId()))
                {
                    return Result.Fail("Já existe um orçamento geral lançado");
                }
            }

            if (!orcamento.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }

            var isValorValido = await ValidaSomaOrcamentoAsync(orcamento.ValorLimite, 0,orcamento.CategoriaId);
            if (!isValorValido)
            {
                return Result.Fail(mensagemErro);
            }


           // orcamento.ValorLimite = orcamento.ValorLimite;
            orcamento.UsuarioId = appIdentityUser.GetUserId();
           // orcamento.CategoriaId = orcamento.CategoriaId;

            await repository.CreateAsync(orcamento);

            var result = Result.Ok()
                .WithSuccess("Orçamento criado com sucesso!");

            return Result.Ok(new GlobalResult(default, string.Join(", ", result.Errors.Select(e => e.Message))));
        }

        public async Task<IEnumerable<OrcamentoResponse>> GetAllWithDescriptionAsync()
        {
            return await repository.GetOrcamentoWithCategoriaAsync(appIdentityUser.GetUserId());
        }

        public async Task<bool> ValidaSomaOrcamentoAsync(decimal valorNovo,decimal valorAntigo, int? categoriaId)
        {
            mensagemErro = string.Empty;
            var orcamentoGeral = await repository.GetGeralAsync(appIdentityUser.GetUserId());
            var valorOrcamentoGeral = orcamentoGeral?.ValorLimite??0;
            var orcamentos = await GetAllAsync();
            var totalOrcamentoCategoria = orcamentos.Where(x=>x.CategoriaId!=null).Sum(o => o.ValorLimite);
            
            if (categoriaId == null)
            {
                valorOrcamentoGeral= valorOrcamentoGeral + valorNovo - valorAntigo;
            }
            else
            {
                totalOrcamentoCategoria = totalOrcamentoCategoria + valorNovo - valorAntigo;
            }


            if (valorOrcamentoGeral > 0 && totalOrcamentoCategoria > valorOrcamentoGeral)
            {
                mensagemErro = $"A soma dos orçamentos das categorias é de {FormatoMoeda.ParaReal(totalOrcamentoCategoria)} está superior que o orçamento Geral {FormatoMoeda.ParaReal(valorOrcamentoGeral)}";
                return false;
            }
            return true;
        }
    }
}