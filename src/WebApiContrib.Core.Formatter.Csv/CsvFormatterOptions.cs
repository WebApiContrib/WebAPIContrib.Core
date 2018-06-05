namespace WebApiContrib.Core.Formatter.Csv
{
    public class CsvFormatterOptions
    {
        public bool UseSingleLineHeaderInCsv { get; set; } = true;

        public string CsvDelimiter { get; set; } = ";";

        public string Encoding { get; set; } = "ISO-8859-1";
        
        public bool UseJsonPropertyJsonIgnoreAttributes { get; set; } = true;
    }
}
