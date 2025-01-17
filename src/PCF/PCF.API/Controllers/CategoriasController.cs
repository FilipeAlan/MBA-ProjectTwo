using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.API.Dto;
using PCF.Core.Entities;
using PCF.Core.Interface;

namespace PCF.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriasController(ICategoriaService categoriaService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<Ok<IEnumerable<CategoriaResponseViewModel>>> GetAll()
        {
            var list = await categoriaService.GetAllAsync();
            return TypedResults.Ok(list.Adapt<IEnumerable<CategoriaResponseViewModel>>());
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<Results<Ok<CategoriaResponseViewModel>, NotFound>> GetById(int id)
        {
            var categoria = await categoriaService.GetByIdAsync(id);

            if (categoria is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(categoria.Adapt<CategoriaResponseViewModel>());
        }

        [HttpPut("{id}")]
        public async Task<Results<NotFound, BadRequest<List<IError>>, NoContent>> Edit(int id, CategoriaRequestViewModel categoria)
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
                return TypedResults.BadRequest(result.Errors);
            }

            return TypedResults.NoContent();
        }

        [HttpPost]
        public async Task<Results<BadRequest<List<IError>>, CreatedAtRoute<CategoriaRequestViewModel>>> AddNew(CategoriaRequestViewModel categoria)
        {
            var result = await categoriaService.AddAsync(categoria.Adapt<Categoria>());

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors);
            }

            return TypedResults.CreatedAtRoute(categoria, nameof(GetById), new { id = result.Value });
        }

        [HttpDelete("{id}")]
        public async Task<Results<NotFound, BadRequest<List<IError>>, NoContent>> Delete(int id)
        {
            var categoria = await categoriaService.GetByIdAsync(id);

            if (categoria is null)
            {
                return TypedResults.NotFound();
            }

            var result = await categoriaService.DeleteAsync(id);

            if (result.IsFailed)
            {
                return TypedResults.BadRequest(result.Errors);
            }

            return TypedResults.NoContent();
        }
    }
}