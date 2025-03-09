using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCF.Core.Dtos.Relatorio
{
    public class RelatorioGastoPorCategoriaResponse
    {
        public int CategoriaId { get; set; }
        public string? Categoria { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorLimite { get; set; }
    }
}
