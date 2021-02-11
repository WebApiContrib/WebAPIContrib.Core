using System.Text;

namespace WebApiContrib.Core.Formatter.Csv
{
    public class CsvFormatterOptions
    {
        public bool UseSingleLineHeaderInCsv { get; set; } = true;

        public string CsvDelimiter { get; set; } = ";";

        public Encoding Encoding { get; set; } = Encoding.Default;

        public bool IncludeExcelDelimiterHeader { get; set; } = false;

        public bool ReplaceLineBreakCharacters { get; set; } = true;
    }
}
