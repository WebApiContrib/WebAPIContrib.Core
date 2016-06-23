using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApiContrib.Core.TagHelpers.Markdown
{
    [HtmlTargetElement("md-extended")]
    public class MarkdownExtendedTagHelper : MarkdownTagHelperBase
    {
        private static readonly MarkdownPipeline Pipeline =
            new MarkdownPipelineBuilder().UseAdvancedExtensions().UseEmojiAndSmiley().Build();

        public override MarkdownPipeline GetPipeline() => Pipeline;

        public override string Name => "md-extended";
    }
}