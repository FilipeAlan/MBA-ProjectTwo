using FluentAssertions;
using Moq;
using PCF.Core.Dtos.Dashboard;
using PCF.Core.Interface;
using PCF.Core.Services;

namespace PCF.Core.Tests.UnitTests
{
    public class DashboardServiceTests
    {
        private readonly Mock<IDashboardRepository> _mockDashboardRepository;
        private readonly Mock<IAppIdentityUser> _mockAppIdentityUser;
        private readonly DashboardService _dashboardService;

        public DashboardServiceTests()
        {
            _mockDashboardRepository = new Mock<IDashboardRepository>();
            _mockAppIdentityUser = new Mock<IAppIdentityUser>();

            _dashboardService = new DashboardService(
                _mockDashboardRepository.Object,
                _mockAppIdentityUser.Object);
        }

        [Fact]
        public async Task ObterHistoricoMensalAsync_ShouldReturnHistoricoMensal()
        {
            // Arrange
            var userId = 1;
            var historicoMensalResponse = new DashboardHistoricoMensalResponse
            (
                [
                    new DashboardHistoricoMensal ( Entradas : 1000, Saidas : 500, Periodo : DateOnly.FromDateTime(DateTime.Now ))
                ]
            );

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockDashboardRepository.Setup(x => x.ObterHistoricoMensalAsync(userId)).ReturnsAsync(historicoMensalResponse);

            // Act
            var result = await _dashboardService.ObterHistoricoMensalAsync();

            // Assert
            result.Should().BeEquivalentTo(historicoMensalResponse);
        }

        [Fact]
        public async Task ObterResumoAsync_ShouldReturnDashboardSummary()
        {
            // Arrange
            var userId = 1;
            var request = new DashboardSummaryRequest(Periodo: DateOnly.FromDateTime(DateTime.Now));
            var dashboardSummary = new DashboardSummary(Entradas: 1000, Saidas: 500);

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockDashboardRepository.Setup(x => x.ObterResumoAsync(request, userId)).ReturnsAsync(dashboardSummary);

            // Act
            var result = await _dashboardService.ObterResumoAsync(request);

            // Assert
            result.Should().BeEquivalentTo(dashboardSummary);
        }

        [Fact]
        public async Task ObterTransacoesAgrupadasPorCategoriaAsync_ShouldReturnTransacoesAgrupadasPorCategoria()
        {
            // Arrange
            var userId = 1;
            var request = new DashboardTransacoesPorCategoriaRequest(Periodo: DateOnly.FromDateTime(DateTime.Now), Tipo: Enumerables.TipoEnum.Entrada);
            var transacoesPorCategoriaResponse = new DashboardTransacoesPorCategoriaResponse
            (
                Categorias:
                [
                    new DashboardTransacaoAgrupadaPorCategoria ( NomeCategoria : "Alimentação", Valor : 500 )
                ]
            );

            _mockAppIdentityUser.Setup(x => x.GetUserId()).Returns(userId);
            _mockDashboardRepository.Setup(x => x.ObterTransacoesAgrupadasPorCategoriaAsync(request, userId)).ReturnsAsync(transacoesPorCategoriaResponse);

            // Act
            var result = await _dashboardService.ObterTransacoesAgrupadasPorCategoriaAsync(request);

            // Assert
            result.Should().BeEquivalentTo(transacoesPorCategoriaResponse);
        }
    }
}