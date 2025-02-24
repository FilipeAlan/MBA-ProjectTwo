using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos.Global;
using PCF.Core.Dtos.Orcamento;
using PCF.Core.Entities;
using PCF.Core.Extensions;
using PCF.Core.Interface;

namespace PCF.API.Controllers
{
    [Route("api/[controller]")]
    public class OrcamentosController(IOrcamentoService orcamentoService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<Ok<IEnumerable<OrcamentoResponse>>> GetAllWithCategorias()
        {
            var list = await orcamentoService.GetAllWithDescriptionAsync();
            return TypedResults.Ok(list.Adapt<IEnumerable<OrcamentoResponse>>());
        }

        [HttpGet("{id}", Name = "ObterOrcamentoPorId")]
        public async Task<Results<Ok<OrcamentoResponse>, NotFound>> GetById(int id)
        {
            var orcamento = await orcamentoService.GetByIdAsync(id);

            if (orcamento is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(orcamento.Adapt<OrcamentoResponse>());
        }

        [HttpPut("{id}")]
        public async Task<Results<NotFound, BadRequest<List<string>>, Ok<GlobalResult>>> Edit(int id, OrcamentoRequest orcamento)
        {

            if (await orcamentoService.GetByIdAsync(id) is null)
            {
                return TypedResults.NotFound();
            }

            var orcamentoEntity = orcamento.Adapt<Orcamento>();
            orcamentoEntity.Id = id;

            var result = await orcamentoService.UpdateAsync(orcamentoEntity);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.Ok(result.Value);
        }

        [HttpPost]
        public async Task<Results<BadRequest<List<string>>, Ok<GlobalResult>>> AddNew(OrcamentoRequest orcamento)
        {
            var result = await orcamentoService.AddAsync(orcamento.Adapt<Orcamento>());

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<Results<NotFound, BadRequest<List<string>>, NoContent>> Delete(int id)
        {
            var orcamento = await orcamentoService.GetByIdAsync(id);

            if (orcamento is null)
            {
                return TypedResults.NotFound();
            }

            var result = await orcamentoService.DeleteAsync(id);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.NoContent();
        }
    }
}