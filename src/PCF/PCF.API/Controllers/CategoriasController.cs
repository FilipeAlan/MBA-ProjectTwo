using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos.Categoria;
using PCF.Core.Entities;
using PCF.Core.Extensions;
using PCF.Core.Interface;

namespace PCF.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriasController(ICategoriaService categoriaService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<Ok<IEnumerable<CategoriaResponse>>> GetAll()
        {
            var list = await categoriaService.GetAllAsync();
            return TypedResults.Ok(list.Adapt<IEnumerable<CategoriaResponse>>());
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<Results<Ok<CategoriaResponse>, NotFound>> GetById(int id)
        {
            var categoria = await categoriaService.GetByIdAsync(id);

            if (categoria is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(categoria.Adapt<CategoriaResponse>());
        }

        [HttpPut("{id}")]
        public async Task<Results<NotFound, BadRequest<List<string>>, NoContent>> Edit(int id, CategoriaRequest categoria)
        {

            if (await categoriaService.GetByIdAsync(id) is null)
            {
                return TypedResults.NotFound();
            }

            var categoriaEntity = categoria.Adapt<Categoria>();
            categoriaEntity.Id = id;

            var result = await categoriaService.UpdateAsync(categoriaEntity);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.NoContent();
        }

        [HttpPost]
        public async Task<Results<BadRequest<List<string>>, CreatedAtRoute<CategoriaRequest>>> AddNew(CategoriaRequest categoria)
        {
            var result = await categoriaService.AddAsync(categoria.Adapt<Categoria>());

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.CreatedAtRoute(categoria, nameof(GetById), new { id = result.Value });
        }

        [HttpDelete("{id}")]
        public async Task<Results<NotFound, BadRequest<List<string>>, NoContent>> Delete(int id)
        {
            var categoria = await categoriaService.GetByIdAsync(id);

            if (categoria is null)
            {
                return TypedResults.NotFound();
            } 

            if (await categoriaService.HasBudgetAssociationAsync(categoria))
            {
                return TypedResults.BadRequest(new List<string> { "Categoria possui orçamentos associados e não pode ser excluida." });
            }

            var result = await categoriaService.DeleteAsync(id);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors.AsErrorList());
            }

            return TypedResults.NoContent();
        }
    }
}