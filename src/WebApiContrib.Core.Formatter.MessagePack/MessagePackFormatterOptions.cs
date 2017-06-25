using MessagePack;

namespace WebApiContrib.Core.Formatter.Csv
{
    public class MessagePackFormatterOptions
    {
        public IFormatterResolver FormatterResolver { get; set; } = MessagePackSerializer.DefaultResolver;
    }
}
