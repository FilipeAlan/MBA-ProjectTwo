using System.Globalization;

namespace PCF.Core.Globalization
{
    public static class FormatoMoeda
    {
        private static readonly CultureInfo cultura = new CultureInfo("pt-BR");

        static FormatoMoeda()
        {
            cultura.NumberFormat.CurrencySymbol = "R$";
        }

        public static string ParaReal(decimal valor)
        {
            return valor.ToString("C", cultura);
        }

        public static decimal ParaDecimal(string valor)
        {
            if (decimal.TryParse(valor, NumberStyles.Currency, cultura, out decimal resultado))
            {
                return resultado;
            }
            return 0;
        }
    }
}