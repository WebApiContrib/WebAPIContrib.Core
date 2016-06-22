using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApiContrib.Core.TagHelpers.Markdown
{
    public abstract class MarkdownTagHelperBase : TagHelper
    {
        public abstract MarkdownPipeline GetPipeline();

        public abstract string Name { get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var pipeline = GetPipeline();

            // get rid of the surrounding tag
            output.TagName = output.TagName != Name ? output.TagName : null;
            var content = await output.GetChildContentAsync();
            var markdown = content.GetContent();

            if (!string.IsNullOrWhiteSpace(markdown))
            {
                var html = Markdig.Markdown.ToHtml(markdown, pipeline);
                output.Content.SetHtmlContent(html);
            }
            else
            {
                output.Content.SetHtmlContent(string.Empty);
            }
        }
    }
}