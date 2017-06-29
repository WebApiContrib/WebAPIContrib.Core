
using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Protobuf
{
    public class ProtobufFormatterOptions
    {
        public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/x-protobuf", "application/protobuf", "application/x-google-protobuf" };

        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "proto" };

       public bool SuppressReadBuffering { get; set; } = false;
    }
}
