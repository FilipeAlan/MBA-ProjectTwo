namespace PCF.Core.Extensions
{
    public class Warning : IReason
    {
        public string Message { get; }
        public Dictionary<string, object> Metadata { get; } = new();
        public string Type => "Warning";

        public Warning(string message)
        {
            Message = message;
        }
    }
}
