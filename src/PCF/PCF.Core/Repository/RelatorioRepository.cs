using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Interface;
using PCF.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCF.Core.Repository
{
    public class RelatorioRepository(PCFDBContext _dbContext) : IRelatorioRepository
    {
        public async Task<List<RelatorioOrcamentoResponse>> GetOrcamentoRealizadoAsync(DateTime dataInicial, DateTime dataFinal,int usarioId)
        {
            if (dataInicial > dataFinal)
            {
                throw new ArgumentException("A data inicial não pode ser maior que a data final.");
            }

            // todo incluir o filtro de usuario
            // como saber o usuarioId logado?

            var query = _dbContext.Transacoes
                .Include(x => x.Categoria)
                .Include(x => x.Usuario)
                .Where(x => x.DataLancamento >= dataInicial && x.DataLancamento <= dataFinal && x.UsuarioId==usarioId );

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
                Usuario = t.Usuario.Nome
            }).ToList();

            return listaOrcamento;
        }
       

    }
}
