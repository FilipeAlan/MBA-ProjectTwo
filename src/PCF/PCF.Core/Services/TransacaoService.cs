using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class TransacaoService(IAppIdentityUser appIdentityUser, ITransacaoRepository repository) : ITransacaoService
    {
        public async Task<Result<int>> AddAsync(Transacao Transacao)
        {
            ArgumentNullException.ThrowIfNull(Transacao);


            Transacao.UsuarioId = appIdentityUser.GetUserId();

            if (!Transacao.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }
            await repository.CreateAsync(Transacao);
            return Result.Ok(Transacao.Id);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var Transacao = await GetByIdAsync(id);

            if (Transacao is null)
            {
                return Result.Fail("Transacao inexistente");
            }

            await repository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<IEnumerable<Transacao>> GetAllAsync()
        {
            return await repository.GetAllAsync(appIdentityUser.GetUserId());
        }

        public async Task<IEnumerable<Transacao>> GetAllByCategoriaAsync(int categoriaId)
        {
            return await repository.GetAllByCategoriaAsync(appIdentityUser.GetUserId(), categoriaId);
        }

        public async Task<IEnumerable<Transacao>> GetAllByPeriodoAsync(DateTime dataInicio, DateTime? dataFin)
        {
            return await repository.GetAllByPeriodoAsync(appIdentityUser.GetUserId(), dataInicio, dataFin);
        }

        public async Task<IEnumerable<Transacao>> GetAllByTipoAsync(TipoEnum tipo)
        {
            return await repository.GetAllByTipoAsync(appIdentityUser.GetUserId(), tipo);
        }

        public async Task<Transacao?> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id, appIdentityUser.GetUserId());
        }

        public async Task<Result> UpdateAsync(Transacao Transacao)
        {
            ArgumentNullException.ThrowIfNull(Transacao);

            var TransacaoExistente = await GetByIdAsync(Transacao.Id);

            if (TransacaoExistente is null)
            {
                return Result.Fail("Transação não encontrada");
            }

            if (!Transacao.Validar())
            {
                return Result.Fail("O valor deve ser maior que zero (0)");
            }

            TransacaoExistente.Descricao = Transacao.Descricao;
            TransacaoExistente.Valor = Transacao.Valor;
            TransacaoExistente.Tipo = Transacao.Tipo;

            await repository.UpdateAsync(TransacaoExistente);

            return Result.Ok();
        }
    }
}