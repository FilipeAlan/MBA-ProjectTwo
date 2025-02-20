using FluentAssertions;
using Moq;
using PCF.Core.Entities;
using PCF.Core.Interface;
using PCF.Core.Services;

namespace PCF.Core.Tests.UnitTests
{
    public class CategoriaServiceTests
    {
        private readonly Mock<IAppIdentityUser> _mockAppIdentityUser;
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
        private readonly CategoriaService _categoriaService;

        public CategoriaServiceTests()
        {
            _mockAppIdentityUser = new Mock<IAppIdentityUser>();
            _mockCategoriaRepository = new Mock<ICategoriaRepository>();

            _categoriaService = new CategoriaService(
                _mockAppIdentityUser.Object,
                _mockCategoriaRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategorias()
        {
            // Arrange
            var userId = 1;
            var categorias = new List<Categoria>
            {
                new() { Id = 1, Nome = "Categoria 1", UsuarioId = userId },
                new() { Id = 2, Nome = "Categoria 2", UsuarioId = userId }
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetAllAsync(userId)).ReturnsAsync(categorias);

            // Act
            var result = await _categoriaService.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(categorias);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategoria_WhenCategoriaExists()
        {
            // Arrange
            var userId = 1;
            var categoriaId = 1;
            var categoria = new Categoria { Id = categoriaId, Nome = "Categoria 1", UsuarioId = userId };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoriaId, userId)).ReturnsAsync(categoria);

            // Act
            var result = await _categoriaService.GetByIdAsync(categoriaId);

            // Assert
            result.Should().BeEquivalentTo(categoria);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCategoriaDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var categoriaId = 1;

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoriaId, userId)).ReturnsAsync((Categoria?)null);

            // Act
            var result = await _categoriaService.GetByIdAsync(categoriaId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailResult_WhenCategoriaWithSameNameExists()
        {
            // Arrange
            var userId = 1;
            var categoria = new Categoria { Nome = "Categoria 1" };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.CheckIfExistsByNomeAsync(default, categoria.Nome, userId)).ReturnsAsync(true);

            // Act
            var result = await _categoriaService.AddAsync(categoria);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Já existe uma categoria com este nome");
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccessResult_WhenCategoriaIsAdded()
        {
            // Arrange
            var userId = 1;
            var categoria = new Categoria { Nome = "Categoria 1" };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.CheckIfExistsByNomeAsync(default, categoria.Nome, userId)).ReturnsAsync(false);
            _mockCategoriaRepository.Setup(x => x.CreateAsync(categoria)).ReturnsAsync(categoria);

            // Act
            var result = await _categoriaService.AddAsync(categoria);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(categoria.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailResult_WhenCategoriaDoesNotExist()
        {
            // Arrange
            var categoriaId = 1;

            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoriaId, It.IsAny<int>())).ReturnsAsync((Categoria?)null);

            // Act
            var result = await _categoriaService.DeleteAsync(categoriaId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Categoria inexistente");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailResult_WhenCategoriaHasTransacoes()
        {
            // Arrange
            var userId = 1;
            var categoriaId = 1;
            var categoria = new Categoria { Id = categoriaId, Nome = "Categoria 1", UsuarioId = userId, Transacoes = new List<Transacao> { new() { CategoriaId = categoriaId, UsuarioId = userId, Usuario = null!, Categoria = null! } } };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoriaId, userId)).ReturnsAsync(categoria);

            // Act
            var result = await _categoriaService.DeleteAsync(categoriaId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Categoria possui transações. Para removê-la, primeiro altere as categorias das transações existentes.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccessResult_WhenCategoriaIsDeleted()
        {
            // Arrange
            var userId = 1;
            var categoriaId = 1;
            var categoria = new Categoria { Id = categoriaId, Nome = "Categoria 1", UsuarioId = userId, Transacoes = new List<Transacao>() };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoriaId, userId)).ReturnsAsync(categoria);
            _mockCategoriaRepository.Setup(x => x.DeleteAsync(categoriaId)).ReturnsAsync(true);

            // Act
            var result = await _categoriaService.DeleteAsync(categoriaId);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailResult_WhenCategoriaDoesNotExist()
        {
            // Arrange
            var categoria = new Categoria { Id = 1, Nome = "Categoria 1" };

            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoria.Id, It.IsAny<int>())).ReturnsAsync((Categoria?)null);

            // Act
            var result = await _categoriaService.UpdateAsync(categoria);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Categoria inexistente");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailResult_WhenCategoriaWithSameNameExists()
        {
            // Arrange
            var userId = 1;
            var categoria = new Categoria { Id = 1, Nome = "Categoria 1" };
            var categoriaExistente = new Categoria { Id = 1, Nome = "Categoria 1", UsuarioId = userId };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoria.Id, userId)).ReturnsAsync(categoriaExistente);
            _mockCategoriaRepository.Setup(x => x.CheckIfExistsByNomeAsync(categoriaExistente.Id, categoria.Nome, userId)).ReturnsAsync(true);

            // Act
            var result = await _categoriaService.UpdateAsync(categoria);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Já existe uma categoria com este nome");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccessResult_WhenCategoriaIsUpdated()
        {
            // Arrange
            var userId = 1;
            var categoria = new Categoria { Id = 1, Nome = "Categoria 1" };
            var categoriaExistente = new Categoria { Id = 1, Nome = "Categoria 1", UsuarioId = userId };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(categoria.Id, userId)).ReturnsAsync(categoriaExistente);
            _mockCategoriaRepository.Setup(x => x.CheckIfExistsByNomeAsync(categoriaExistente.Id, categoria.Nome, userId)).ReturnsAsync(false);
            _mockCategoriaRepository.Setup(x => x.UpdateAsync(categoriaExistente)).ReturnsAsync(categoriaExistente);

            // Act
            var result = await _categoriaService.UpdateAsync(categoria);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}