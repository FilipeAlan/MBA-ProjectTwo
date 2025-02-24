using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace PCF.API.Services
{
    public class ExcelExportService
    {
        public byte[] GenerateExcelFromTable<T>(List<T> data, string sheetName = "Relatório Excel")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties();

            // Cabeçalhos da Tabela
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cell(1, col + 1).Value = properties[col].Name;
                worksheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            // Adiciona os dados na planilha
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(data[row])?.ToString() ?? "";
                    worksheet.Cell(row + 2, col + 1).Value = value;
                }
            }

            // Ajusta largura das colunas automaticamente
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
