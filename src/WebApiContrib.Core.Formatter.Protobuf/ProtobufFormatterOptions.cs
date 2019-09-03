
using System;
using System.Collections.Generic;

namespace WebApiContrib.Core.Formatter.Protobuf
{
    public class ProtobufFormatterOptions
    {
        private readonly Dictionary<Type, Type> _surrogates = new Dictionary<Type, Type>();

        public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/x-protobuf", "application/protobuf", "application/x-google-protobuf" };

        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "proto" };

        public IReadOnlyDictionary<Type, Type> Surrogates => _surrogates;

       public bool SuppressReadBuffering { get; set; } = false;

       public void AddSurrogate(Type modelType, Type surrogateType)
       {
           _surrogates.Add(modelType, surrogateType);
       }
    }
}
