using FluentAssertions;
using Moq;
using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Interface;
using PCF.Core.Services;

namespace PCF.Core.Tests.UnitTests
{
    public class TransacaoServiceTests
    {
        private readonly Mock<IAppIdentityUser> _mockAppIdentityUser;
        private readonly Mock<ITransacaoRepository> _mockTransacaoRepository;
        private readonly Mock<IOrcamentoRepository> _mockOrcamentoRepository;
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
        private readonly TransacaoService _transacaoService;

        public TransacaoServiceTests()
        {
            _mockAppIdentityUser = new Mock<IAppIdentityUser>();
            _mockTransacaoRepository = new Mock<ITransacaoRepository>();
            _mockOrcamentoRepository = new Mock<IOrcamentoRepository>();
            _mockCategoriaRepository = new Mock<ICategoriaRepository>();

            _transacaoService = new TransacaoService(
                _mockAppIdentityUser.Object,
                _mockTransacaoRepository.Object,
                _mockOrcamentoRepository.Object,
                _mockCategoriaRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTransacoes()
        {
            // Arrange
            var userId = 1;
            var transacoes = new List<Transacao>
            {
                new Transacao { Id = 1, Valor = 100, UsuarioId = userId , Categoria = null!, Usuario= null!},
                new Transacao { Id = 2, Valor = 200, UsuarioId = userId , Categoria = null!, Usuario= null!}
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetAllAsync(userId)).ReturnsAsync(transacoes);

            // Act
            var result = await _transacaoService.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(transacoes);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTransacao_WhenTransacaoExists()
        {
            // Arrange
            var userId = 1;
            var transacaoId = 1;
            var transacao = new Transacao { Id = transacaoId, Valor = 100, UsuarioId = userId, Categoria = null!, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetByIdAsync(transacaoId, userId)).ReturnsAsync(transacao);

            // Act
            var result = await _transacaoService.GetByIdAsync(transacaoId);

            // Assert
            result.Should().BeEquivalentTo(transacao);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTransacaoDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var transacaoId = 1;

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetByIdAsync(transacaoId, userId)).ReturnsAsync((Transacao?)null);

            // Act
            var result = await _transacaoService.GetByIdAsync(transacaoId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailResult_WhenTransacaoIsInvalid()
        {
            // Arrange
            var userId = 1;
            var transacao = new Transacao { Valor = -100, UsuarioId = userId, Categoria = null!, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);

            // Act
            var result = await _transacaoService.AddAsync(transacao);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "O valor deve ser maior que zero (0)");
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccessResult_WhenTransacaoIsAdded()
        {
            // Arrange
            var userId = 1;
            var transacao = new Transacao { Valor = 100, UsuarioId = userId, Categoria = null!, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.CreateAsync(transacao)).ReturnsAsync(transacao);

            // Act
            var result = await _transacaoService.AddAsync(transacao);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailResult_WhenTransacaoDoesNotExist()
        {
            // Arrange
            var transacaoId = 1;

            _mockTransacaoRepository.Setup(x => x.GetByIdAsync(transacaoId, It.IsAny<int>())).ReturnsAsync((Transacao?)null);

            // Act
            var result = await _transacaoService.DeleteAsync(transacaoId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Transacao inexistente");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccessResult_WhenTransacaoIsDeleted()
        {
            // Arrange
            var userId = 1;
            var transacaoId = 1;
            var transacao = new Transacao { Id = transacaoId, Valor = 100, UsuarioId = userId, Categoria = null!, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetByIdAsync(transacaoId, userId)).ReturnsAsync(transacao);
            _mockTransacaoRepository.Setup(x => x.DeleteAsync(transacaoId)).ReturnsAsync(true);

            // Act
            var result = await _transacaoService.DeleteAsync(transacaoId);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailResult_WhenTransacaoDoesNotExist()
        {
            // Arrange
            var transacao = new Transacao { Id = 1, Valor = 100, Categoria = null!, Usuario = null! };

            _mockTransacaoRepository.Setup(x => x.GetByIdAsync(transacao.Id, It.IsAny<int>())).ReturnsAsync((Transacao?)null);

            // Act
            var result = await _transacaoService.UpdateAsync(transacao);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(x => x.Message == "Transação não encontrada");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccessResult_WhenTransacaoIsUpdated()
        {
            // Arrange
            var userId = 1;
            var transacao = new Transacao { Id = 1, Valor = 100, Categoria = null!, Usuario = null! };
            var transacaoExistente = new Transacao { Id = 1, Valor = 50, UsuarioId = userId, Categoria = null!, Usuario = null! };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetByIdAsync(transacao.Id, userId)).ReturnsAsync(transacaoExistente);
            _mockTransacaoRepository.Setup(x => x.UpdateAsync(transacaoExistente)).ReturnsAsync(transacaoExistente);

            // Act
            var result = await _transacaoService.UpdateAsync(transacao);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllByCategoriaAsync_ShouldReturnTransacoesByCategoria()
        {
            // Arrange
            var userId = 1;
            var categoriaId = 1;
            var transacoes = new List<Transacao>
            {
                new Transacao { Id = 1, Valor = 100, CategoriaId = categoriaId, UsuarioId = userId , Categoria = null!, Usuario= null!},
                new Transacao { Id = 2, Valor = 200, CategoriaId = categoriaId, UsuarioId = userId , Categoria = null!, Usuario= null!}
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetAllByCategoriaAsync(userId, categoriaId)).ReturnsAsync(transacoes);

            // Act
            var result = await _transacaoService.GetAllByCategoriaAsync(categoriaId);

            // Assert
            result.Should().BeEquivalentTo(transacoes);
        }

        [Fact]
        public async Task GetAllByPeriodoAsync_ShouldReturnTransacoesByPeriodo()
        {
            // Arrange
            var userId = 1;
            var dataInicio = new DateTime(2023, 1, 1);
            var dataFin = new DateTime(2023, 12, 31);
            var transacoes = new List<Transacao>
            {
                new Transacao { Id = 1, Valor = 100, DataLancamento = new DateTime(2023, 1, 15), UsuarioId = userId , Categoria = null!, Usuario= null!},
                new Transacao { Id = 2, Valor = 200, DataLancamento = new DateTime(2023, 6, 15), UsuarioId = userId , Categoria = null!, Usuario= null!}
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetAllByPeriodoAsync(userId, dataInicio, dataFin)).ReturnsAsync(transacoes);

            // Act
            var result = await _transacaoService.GetAllByPeriodoAsync(dataInicio, dataFin);

            // Assert
            result.Should().BeEquivalentTo(transacoes);
        }

        [Fact]
        public async Task GetAllByTipoAsync_ShouldReturnTransacoesByTipo()
        {
            // Arrange
            var userId = 1;
            var tipo = TipoEnum.Entrada;
            var transacoes = new List<Transacao>
            {
                new Transacao { Id = 1, Valor = 100, Tipo = tipo, UsuarioId = userId , Categoria = null!, Usuario= null!},
                new Transacao { Id = 2, Valor = 200, Tipo = tipo, UsuarioId = userId , Categoria = null!, Usuario= null!}
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockTransacaoRepository.Setup(x => x.GetAllByTipoAsync(userId, tipo)).ReturnsAsync(transacoes);

            // Act
            var result = await _transacaoService.GetAllByTipoAsync(tipo);

            // Assert
            result.Should().BeEquivalentTo(transacoes);
        }
    }
}