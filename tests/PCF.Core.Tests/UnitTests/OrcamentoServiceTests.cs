using FluentAssertions;
using Moq;
using PCF.Core.Dtos.Orcamento;
using PCF.Core.Entities;
using PCF.Core.Interface;
using PCF.Core.Services;

namespace PCF.Core.Tests.UnitTests
{
    public class OrcamentoServiceTests
    {
        private readonly Mock<IAppIdentityUser> _mockAppIdentityUser;
        private readonly Mock<IOrcamentoRepository> _mockOrcamentoRepository;
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
        private readonly OrcamentoService _orcamentoService;

        public OrcamentoServiceTests()
        {
            _mockAppIdentityUser = new Mock<IAppIdentityUser>();
            _mockOrcamentoRepository = new Mock<IOrcamentoRepository>();
            _mockCategoriaRepository = new Mock<ICategoriaRepository>();

            _orcamentoService = new OrcamentoService(
                _mockAppIdentityUser.Object,
                _mockOrcamentoRepository.Object,
                _mockCategoriaRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrcamentos()
        {
            // Arrange
            var userId = 1;
            var orcamentos = new List<Orcamento>
            {
                new() { Id = 1, ValorLimite = 1000, UsuarioId = userId, Usuario = null! },
                new() { Id = 2, ValorLimite = 2000, UsuarioId = userId, Usuario = null! }
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockOrcamentoRepository.Setup(x => x.GetAllAsync(userId)).ReturnsAsync(orcamentos);

            // Act
            var result = await _orcamentoService.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(orcamentos);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrcamento_WhenOrcamentoExists()
        {
            // Arrange
            var userId = 1;
            var orcamentoId = 1;
            var orcamento = new Orcamento { Id = orcamentoId, ValorLimite = 1000, UsuarioId = userId, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockOrcamentoRepository.Setup(x => x.GetByIdAsync(orcamentoId, userId)).ReturnsAsync(orcamento);

            // Act
            var result = await _orcamentoService.GetByIdAsync(orcamentoId);

            // Assert
            result.Should().BeEquivalentTo(orcamento);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenOrcamentoDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var orcamentoId = 1;

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockOrcamentoRepository.Setup(x => x.GetByIdAsync(orcamentoId, userId)).ReturnsAsync((Orcamento?)null);

            // Act
            var result = await _orcamentoService.GetByIdAsync(orcamentoId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailResult_WhenCategoriaIsInvalid()
        {
            // Arrange
            var userId = 1;
            var orcamento = new Orcamento { ValorLimite = 1000, CategoriaId = 1, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(orcamento.CategoriaId.Value, userId)).ReturnsAsync((Categoria?)null);
            _mockCategoriaRepository.Setup(x => x.GetGeralByIdAsync(orcamento.CategoriaId.Value)).ReturnsAsync((Categoria?)null);

            // Act
            var result = await _orcamentoService.AddAsync(orcamento);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Categoria informada é inválida.");
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailResult_WhenOrcamentoAlreadyExistsForCategoria()
        {
            // Arrange
            var userId = 1;
            var orcamento = new Orcamento { ValorLimite = 1000, CategoriaId = 1, Usuario = null! };
            var categoria = new Categoria { Id = 1, Nome = "Categoria 1" };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(orcamento.CategoriaId.Value, userId)).ReturnsAsync(categoria);
            _mockOrcamentoRepository.Setup(x => x.CheckIfExistsByIdAsync(orcamento.CategoriaId.Value, userId)).ReturnsAsync(true);

            // Act
            var result = await _orcamentoService.AddAsync(orcamento);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Já existe um orçamento para essa categoria lançado");
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccessResult_WhenOrcamentoIsAdded()
        {
            // Arrange
            var userId = 1;
            var orcamento = new Orcamento { ValorLimite = 1000, CategoriaId = 1, Usuario = null! };
            var categoria = new Categoria { Id = 1, Nome = "Categoria 1" };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockCategoriaRepository.Setup(x => x.GetByIdAsync(orcamento.CategoriaId.Value, userId)).ReturnsAsync(categoria);
            _mockOrcamentoRepository.Setup(x => x.CheckIfExistsByIdAsync(orcamento.CategoriaId.Value, userId)).ReturnsAsync(false);
            _mockOrcamentoRepository.Setup(x => x.CreateAsync(orcamento)).ReturnsAsync(orcamento);

            // Act
            var result = await _orcamentoService.AddAsync(orcamento);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailResult_WhenOrcamentoDoesNotExist()
        {
            // Arrange
            var orcamentoId = 1;

            _mockOrcamentoRepository.Setup(x => x.GetByIdAsync(orcamentoId, It.IsAny<int>())).ReturnsAsync((Orcamento?)null);

            // Act
            var result = await _orcamentoService.DeleteAsync(orcamentoId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Orçamento inexistente");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccessResult_WhenOrcamentoIsDeleted()
        {
            // Arrange
            var userId = 1;
            var orcamentoId = 1;
            var orcamento = new Orcamento { Id = orcamentoId, ValorLimite = 1000, UsuarioId = userId, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockOrcamentoRepository.Setup(x => x.GetByIdAsync(orcamentoId, userId)).ReturnsAsync(orcamento);
            _mockOrcamentoRepository.Setup(x => x.DeleteAsync(orcamentoId)).ReturnsAsync(true);

            // Act
            var result = await _orcamentoService.DeleteAsync(orcamentoId);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailResult_WhenOrcamentoDoesNotExist()
        {
            // Arrange
            var orcamento = new Orcamento { Id = 1, ValorLimite = 1000, Usuario = null! };

            _mockOrcamentoRepository.Setup(x => x.GetByIdAsync(orcamento.Id, It.IsAny<int>())).ReturnsAsync((Orcamento?)null);

            // Act
            var result = await _orcamentoService.UpdateAsync(orcamento);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Orçamento inexistente");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccessResult_WhenOrcamentoIsUpdated()
        {
            // Arrange
            var userId = 1;
            var orcamento = new Orcamento { Id = 1, ValorLimite = 1000, Usuario = null! };
            var orcamentoExistente = new Orcamento { Id = 1, ValorLimite = 500, UsuarioId = userId, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockOrcamentoRepository.Setup(x => x.GetByIdAsync(orcamento.Id, userId)).ReturnsAsync(orcamentoExistente);
            _mockOrcamentoRepository.Setup(x => x.UpdateAsync(orcamentoExistente)).ReturnsAsync(orcamentoExistente);

            // Act
            var result = await _orcamentoService.UpdateAsync(orcamento);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllWithDescriptionAsync_ShouldReturnAllOrcamentosWithDescription()
        {
            // Arrange
            var userId = 1;
            var orcamentos = new List<OrcamentoResponse>
            {
                new() { OrcamentoId = 1, ValorLimite = 1000, UsuarioId = userId, NomeCategoria = "Categoria 1" },
                new() { OrcamentoId = 2, ValorLimite = 2000, UsuarioId = userId, NomeCategoria = "Categoria 2" }
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockOrcamentoRepository.Setup(x => x.GetOrcamentoWithCategoriaAsync(userId)).ReturnsAsync(orcamentos);

            // Act
            var result = await _orcamentoService.GetAllWithDescriptionAsync();

            // Assert
            result.Should().BeEquivalentTo(orcamentos);
        }
    }
}