using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace WebApiContrib.Core.Formatter.Protobuf
{
    public class ProtobufOutputFormatter :  OutputFormatter
    {
        private readonly ProtobufFormatterOptions _options;

        public ProtobufOutputFormatter(ProtobufFormatterOptions protobufFormatterOptions)
        {
            ContentType = "application/x-protobuf";
            _options = protobufFormatterOptions ?? throw new ArgumentNullException(nameof(protobufFormatterOptions));
            foreach (var contentType in protobufFormatterOptions.SupportedContentTypes)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
        }

        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        public string ContentType { get; private set; }

        public static RuntimeTypeModel Model
        {
            get { return model.Value; }
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            MemoryStream stream = new MemoryStream();
            Model.Serialize(stream, context.Object);

            stream.Position = 0;
            var sr = new StreamReader(stream);
            await response.WriteAsync(sr.ReadToEnd());
        }
    }
}
