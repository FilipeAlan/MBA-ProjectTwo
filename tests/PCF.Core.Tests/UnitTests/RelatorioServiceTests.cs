using FluentAssertions;
using Moq;
using PCF.Core.Dtos.Relatorio;
using PCF.Core.Interface;
using PCF.Core.Services;

namespace PCF.Core.Tests.UnitTests
{
    public class RelatorioServiceTests
    {
        private readonly Mock<IAppIdentityUser> _mockAppIdentityUser;
        private readonly Mock<IRelatorioRepository> _mockRelatorioRepository;
        private readonly RelatorioService _relatorioService;

        public RelatorioServiceTests()
        {
            _mockAppIdentityUser = new Mock<IAppIdentityUser>();
            _mockRelatorioRepository = new Mock<IRelatorioRepository>();

            _relatorioService = new RelatorioService(
                _mockAppIdentityUser.Object,
                _mockRelatorioRepository.Object);
        }

        [Fact]
        public async Task GetOrcamentoRealizado_ShouldReturnOrcamentoRealizado()
        {
            // Arrange
            var userId = 1;
            var dataInicial = new DateTime(2023, 1, 1);
            var dataFinal = new DateTime(2023, 12, 31);
            var orcamentoRealizado = new List<RelatorioOrcamentoResponse>
            {
                new RelatorioOrcamentoResponse
                {
                    TransacaoId = 1,
                    DataLancamento = DateTime.Now,
                    TipoLancamento = "Entrada",
                    Valor = 1000,
                    ValorLimite = 2000,
                    Descricao = "Salário",
                    CategoriaId = 1,
                    Categoria = "Renda",
                    Tipo = "Entrada",
                    UsuarioId = userId,
                    Usuario = "Usuário 1"
                }
            };

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockRelatorioRepository.Setup(x => x.GetOrcamentoRealizadoAsync(dataInicial, dataFinal, userId)).ReturnsAsync(orcamentoRealizado);

            // Act
            var result = await _relatorioService.GetOrcamentoRealizado(dataInicial, dataFinal);

            // Assert
            result.Should().BeEquivalentTo(orcamentoRealizado);
        }
    }
}