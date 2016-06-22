using Markdig;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApiContrib.Core.TagHelpers.Markdown
{
    [HtmlTargetElement("md")]
    public class MarkdownTagHelper : MarkdownTagHelperBase
    {
        public bool PipeTables { get; set; }

        public bool GridTables { get; set; }

        public bool ExtraEmphasis { get; set; }

        public bool DefinitionLists { get; set; }

        public bool Footnotes { get; set; }

        public bool TaskLists { get; set; }

        public bool ExtraBulletLists { get; set; }

        public bool Abbreviations { get; set; }

        public bool Emoji { get; set; }

        public override MarkdownPipeline GetPipeline()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();

            if (PipeTables) pipelineBuilder = pipelineBuilder.UsePipeTables();
            if (GridTables) pipelineBuilder = pipelineBuilder.UseGridTables();
            if (ExtraEmphasis) pipelineBuilder = pipelineBuilder.UseEmphasisExtras();
            if (DefinitionLists) pipelineBuilder = pipelineBuilder.UseDefinitionLists();
            if (Footnotes) pipelineBuilder = pipelineBuilder.UseFootnotes();
            if (TaskLists) pipelineBuilder = pipelineBuilder.UseTaskLists();
            if (ExtraBulletLists) pipelineBuilder = pipelineBuilder.UseListExtras();
            if (Abbreviations) pipelineBuilder = pipelineBuilder.UseAbbreviations();
            if (Emoji) pipelineBuilder = pipelineBuilder.UseEmojiAndSmiley();

            return pipelineBuilder.Build();
        }

        public override string Name => "md";
    }
}
