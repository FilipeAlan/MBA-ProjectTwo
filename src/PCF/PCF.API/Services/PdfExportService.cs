using System.Collections.Generic;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PCF.API.Services
{
    public class PdfExportService
    {
        public byte[] GeneratePdfFromTable<T>(List<T> data, string title)
        {
            using var document = new PdfDocument();
            var page = document.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Arial", 16, XFontStyle.Bold);
            var fontBody = new XFont("Arial", 12, XFontStyle.Regular);

            int yPosition = 40;

            // Adiciona o título
            graphics.DrawString(title, fontTitle, XBrushes.Black, new XPoint(40, yPosition));
            yPosition += 30;

            var properties = typeof(T).GetProperties();

            // Cabeçalho da Tabela
            int xPosition = 40;
            foreach (var prop in properties)
            {
                graphics.DrawString(prop.Name, fontBody, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += 100;
            }

            yPosition += 20;

            // Adiciona os dados da tabela
            foreach (var item in data)
            {
                xPosition = 40;
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item)?.ToString() ?? "";
                    graphics.DrawString(value, fontBody, XBrushes.Black, new XPoint(xPosition, yPosition));
                    xPosition += 100;
                }
                yPosition += 20;
            }

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }
    }
}
