using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace WebApiContrib.Core.Formatter.Protobuf
{
    public class ProtobufInputFormatter : InputFormatter
    {
        private readonly ProtobufFormatterOptions _options;

        public ProtobufInputFormatter(ProtobufFormatterOptions protobufFormatterOptions)
        {
            _options = protobufFormatterOptions ?? throw new ArgumentNullException(nameof(protobufFormatterOptions));
            foreach (var contentType in protobufFormatterOptions.SupportedContentTypes)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }

            foreach (var surrogate in protobufFormatterOptions.Surrogates)
            {
                model.Value.Add(surrogate.Key, false).SetSurrogate(surrogate.Value);
            }
        }

        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        public static RuntimeTypeModel Model
        {
            get { return model.Value; }
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue requestContentType = null;
            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);

            MemoryStream stream = new MemoryStream();
            await request.Body.CopyToAsync(stream);
            stream.Position = 0;
            object result = Model.Deserialize(stream, null, type, stream.Length);
            return await InputFormatterResult.SuccessAsync(result);
        }

        protected override bool CanReadType(Type type)
        {
            return true;
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = RuntimeTypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }
    }
}
