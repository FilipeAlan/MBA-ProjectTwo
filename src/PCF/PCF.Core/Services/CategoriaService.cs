using PCF.Core.Entities;
using PCF.Core.Interface;

namespace PCF.Core.Services
{
    public class CategoriaService(IAppIdentityUser appIdentityUser, ICategoriaRepository repository) : ICategoriaService
    {
        public async Task<Result<int>> AddAsync(Categoria categoria)
        {
            ArgumentNullException.ThrowIfNull(categoria);

            if (await repository.CheckIfExistsByNomeAsync(default, categoria.Nome, appIdentityUser.GetUserId()))
            {
                return Result.Fail<int>("Já existe uma categoria com este nome");
            }

            categoria.UsuarioId = appIdentityUser.GetUserId();
            categoria.Padrao = false;

            await repository.CreateAsync(categoria);
            return Result.Ok(categoria.Id);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var categoria = await GetByIdAsync(id);

            if (categoria is null)
            {
                return Result.Fail("Categoria inexistente");
            }

            if (categoria.Transacoes.Any())
            {
                return Result.Fail("Categoria possui transações. Para removê-la, primeiro altere as categorias das transações existentes.");
            }

            await repository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await repository.GetAllAsync(appIdentityUser.GetUserId());
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id, appIdentityUser.GetUserId());
        }

        public async Task<Result> UpdateAsync(Categoria categoria)
        {
            ArgumentNullException.ThrowIfNull(categoria);

            var categoriaExistente = await GetByIdAsync(categoria.Id);

            if (categoriaExistente is null)
            {
                return Result.Fail("Categoria inexistente");
            }

            if (await repository.CheckIfExistsByNomeAsync(categoriaExistente.Id, categoria.Nome, appIdentityUser.GetUserId()))
            {
                return Result.Fail("Já existe uma categoria com este nome");
            }

            categoriaExistente.Nome = categoria.Nome;
            categoriaExistente.Descricao = categoria.Descricao;

            await repository.UpdateAsync(categoriaExistente);

            return Result.Ok();
        }
    }
}