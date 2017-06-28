using MessagePack;
using MessagePack.Resolvers;
using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.MessagePack
{
    public class MessagePackFormatterOptions
    {
        public IFormatterResolver FormatterResolver { get; set; } = ContractlessStandardResolver.Instance;

        public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/x-msgpack", "application/msgpack" };

        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "mp" };

        public bool SuppressReadBuffering { get; set; } = false;
    }
}
