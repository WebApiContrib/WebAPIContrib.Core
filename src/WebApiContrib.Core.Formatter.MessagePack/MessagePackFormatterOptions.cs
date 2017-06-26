using MessagePack;
using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Csv
{
    public class MessagePackFormatterOptions
    {
        public IFormatterResolver FormatterResolver { get; set; } = MessagePackSerializer.DefaultResolver;

        public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/x-msgpack" };

        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "mp" };

        public bool SuppressReadBufferring { get; set; } = false;
    }
}
