using System;
using FluentAssertions;

using HtmlAgilityPack;

using Microsoft.DotNet.Interactive.Formatting;

using Xunit;

namespace Microsoft.DotNet.Interactive.SandDance.Extension.Tests
{
    public class FormattingTests
    {
        public FormattingTests()
        {
            Formatter.ResetToDefault();
        }

        [Fact]
        public void it_emits_html_fragment()
        {
            SandDanceExtensions.RegisterFormatters();
            var explorer = new DataExplorer(() => new[] { 
                new { a = 12 , b = new DateTime(1977,2,12)},
                new { a = 14 , b = new DateTime(1977,2,13)},
                new { a = 16 , b = new DateTime(1977,2,14)}

            });

            var formatted = explorer.ToDisplayString(HtmlFormatter.MimeType);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(formatted);

            var scripts = htmlDoc.DocumentNode.SelectNodes("//div/script");

            scripts.Count.Should().Be(1);

        }
    }
}
