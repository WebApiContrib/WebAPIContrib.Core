using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace WebApiContrib.Core.Formatter.Yaml
{
    public class YamlFormatterOptions
    {
        public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/x-yaml", "application/yaml", "text/x-yaml", "text/yaml" };
        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "yaml" };
    }
}
