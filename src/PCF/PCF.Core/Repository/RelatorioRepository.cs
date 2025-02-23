using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Dtos.Relatorio;
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
        public async Task<List<RelatorioOrcamentoResponse>> GetOrcamentoRealizadoAsync(DateTime dataInicial, DateTime dataFinal,int usuarioId)
        {
            if (dataInicial > dataFinal)
            {
                throw new ArgumentException("A data inicial não pode ser maior que a data final.");
            }

            var query = _dbContext.Transacoes
                .Include(t => t.Categoria)
                .Include(u => u.Usuario)
                .Where(x => x.DataLancamento >= dataInicial && x.DataLancamento <= dataFinal && x.UsuarioId==usuarioId );

            var transacoes = await query.ToListAsync();

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
                TipoLancamento = t.Tipo == 0 ? "Entrada" : "Saída"
            }).ToList();

            return listaOrcamento;
        }
       

    }
}
