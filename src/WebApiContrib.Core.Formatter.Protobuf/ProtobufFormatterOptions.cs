
using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Protobuf
{
    public class ProtobufFormatterOptions
    {
        //public IFormatterResolver FormatterResolver { get; set; } = ContractlessStandardResolver.Instance;

        public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/x-protobuf", "application/x-protobuf" };

        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "proto" };

       public bool SuppressReadBuffering { get; set; } = false;
    }
}
