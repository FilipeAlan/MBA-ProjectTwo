using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Dtos.Relatorio;
using PCF.Core.Entities;
using PCF.Core.Enumerables;
using PCF.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCF.Core.Repository
{
    public class RelatorioRepository(PCFDBContext _dbContext) : IRelatorioRepository
    {
        public async Task<List<RelatorioOrcamentoResponse>> GetOrcamentoRealizadoAsync(DateTime dataInicial, DateTime dataFinal, int usuarioId)
        {
            if (dataInicial > dataFinal)
            {
                throw new ArgumentException("A data inicial não pode ser maior que a data final.");
            }

            var query = _dbContext.Transacoes
                .Include(t => t.Categoria)
                .Include(u => u.Usuario)
                .Where(x => x.DataLancamento >= dataInicial && x.DataLancamento <= dataFinal && x.UsuarioId == usuarioId);

            var transacoes = await query.ToListAsync();
            var orcamentoCategoria = await _dbContext.Orcamentos.Where(x => x.UsuarioId == usuarioId).ToListAsync();

            var listaOrcamento = transacoes.Select(t => new RelatorioOrcamentoResponse
            {
                TransacaoId = t.Id,
                DataLancamento = t.DataLancamento,
                Valor = t.Valor,
                Descricao = t.Descricao,
                CategoriaId = t.CategoriaId,
                Categoria = t.Categoria.Nome,
                Tipo = t.Tipo.ToString(),
                UsuarioId = t.UsuarioId,
                Usuario = t.Usuario.Nome,
                TipoLancamento = t.Tipo == 0 ? "Entrada" : "Saída",
                ValorLimite = orcamentoCategoria.Where(x => x.CategoriaId == t.CategoriaId).Select(x => x.ValorLimite).FirstOrDefault()
            }).OrderBy(o => o.DataLancamento).ToList();

            return listaOrcamento;
        }

        public async Task<List<RelatorioGastoPorCategoriaResponse>> GetGastoPorCategoriaAsync(DateTime dataInicial, DateTime dataFinal, int usuarioId)
        {
            if (dataInicial > dataFinal)
            {
                throw new ArgumentException("A data inicial não pode ser maior que a data final.");
            }

            var query = _dbContext.Transacoes
                .Include(t => t.Categoria)
                .Where(x => x.DataLancamento >= dataInicial && x.DataLancamento <= dataFinal && x.UsuarioId == usuarioId && x.Tipo == TipoEnum.Saida);

            var transacoesAgrupadas = await query
                .GroupBy(t => t.CategoriaId)
                .Select(g => new RelatorioGastoPorCategoriaResponse
                {
                    CategoriaId = g.Key,
                    Categoria = g.First().Categoria.Nome,
                    ValorTotal = g.Sum(t => t.Valor),
                    ValorLimite = _dbContext.Orcamentos
                        .Where(o => o.CategoriaId == g.Key && o.UsuarioId == usuarioId)
                        .Select(o => o.ValorLimite)
                        .FirstOrDefault()
                }).ToListAsync();

            var orcamentoGeral = await _dbContext.Orcamentos
                .Where(x => x.UsuarioId == usuarioId && x.CategoriaId == null)
                .SumAsync(x => x.ValorLimite);

            if (orcamentoGeral > 0)
            {
                transacoesAgrupadas.Add(new RelatorioGastoPorCategoriaResponse
                {
                    CategoriaId = 0,
                    Categoria = "Orçamento Geral",
                    ValorLimite = orcamentoGeral
                });
            }
            return transacoesAgrupadas;

        }


    }
}
