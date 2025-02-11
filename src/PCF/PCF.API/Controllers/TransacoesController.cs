using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos;
using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Extensions;
using PCF.Core.Interface;

namespace PCF.API.Controllers
{
    [Route("api/transacoes")]
    public class TransacoesController(ITransacaoService TransacaoService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<Ok<IEnumerable<TransacaoResponse>>> GetAll()
        {
            var list = await TransacaoService.GetAllAsync();
            return TypedResults.Ok(list.Adapt<IEnumerable<TransacaoResponse>>());
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<Ok<IEnumerable<TransacaoResponse>>> GetAllByCategoria(int categoriaId)
        {
            var list = await TransacaoService.GetAllByCategoriaAsync(categoriaId);
            return TypedResults.Ok(list.Adapt<IEnumerable<TransacaoResponse>>());
        }

        [HttpGet("tipo/{tipo}")]
        public async Task<Ok<IEnumerable<TransacaoResponse>>> GetAllByTipo(TipoEnum tipo)
        {
            var list = await TransacaoService.GetAllByTipoAsync(tipo);
            return TypedResults.Ok(list.Adapt<IEnumerable<TransacaoResponse>>());
        }

        [HttpGet("periodo/{dataInicio}")]
        public async Task<Ok<IEnumerable<TransacaoResponse>>> GetAllByPeriodo(DateTime dataInicio, DateTime? dataFin)
        {
            var list = await TransacaoService.GetAllByPeriodoAsync(dataInicio, dataFin);
            return TypedResults.Ok(list.Adapt<IEnumerable<TransacaoResponse>>());
        }

        [HttpGet("{id:int}")]
        public async Task<Results<Ok<TransacaoResponse>, NotFound>> GetById(int id)
        {
            var Transacao = await TransacaoService.GetByIdAsync(id);

            if (Transacao is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(Transacao.Adapt<TransacaoResponse>());
        }

        [HttpPost]
        public async Task<Results<BadRequest<List<string>>, CreatedAtRoute<TransacaoRequest>>> AddNew(TransacaoRequest Transacao)
        {
            var result = await TransacaoService.AddAsync(Transacao.Adapt<Transacao>());

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.CreatedAtRoute(Transacao, nameof(GetById), new { id = result.Value });
        }

        [HttpPut("{id}")]
        public async Task<Results<NotFound, BadRequest<List<string>>, NoContent>> Edit(int id, TransacaoRequest Transacao)
        {

            if (await TransacaoService.GetByIdAsync(id) is null)
            {
                return TypedResults.NotFound();
            }

            var TransacaoEntity = Transacao.Adapt<Transacao>();
            TransacaoEntity.Id = id;

            var result = await TransacaoService.UpdateAsync(TransacaoEntity);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<Results<NotFound, BadRequest<List<string>>, NoContent>> Delete(int id)
        {
            var Transacao = await TransacaoService.GetByIdAsync(id);

            if (Transacao is null)
            {
                return TypedResults.NotFound();
            }

            var result = await TransacaoService.DeleteAsync(id);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.NoContent();
        }
    }
}