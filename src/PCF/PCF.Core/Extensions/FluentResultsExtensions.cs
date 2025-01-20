namespace PCF.Core.Extensions
{
    public static class FluentResultsExtensions
    {
        public static List<string> AsErrorList(this List<IError> errors)
        {
            return errors.Select(e => e.Message).ToList();
        }
    }
}