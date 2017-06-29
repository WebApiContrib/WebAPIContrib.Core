using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace WebApiContrib.Core.Formatter.Protobuf
{
    public class ProtobufOutputFormatter :  OutputFormatter
    {
        private readonly ProtobufFormatterOptions _options;

        public ProtobufOutputFormatter()
        {
            ContentType = "application/x-protobuf";
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));

            //SupportedEncodings.Add(Encoding.GetEncoding("utf-8"));
        }

        public ProtobufOutputFormatter(ProtobufFormatterOptions protobufFormatterOptions)
        {
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

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
    
            Model.Serialize(response.Body, context.Object);
            return Task.FromResult(response);
        }
    }
}
