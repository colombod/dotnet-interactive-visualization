using System;
using System.Diagnostics;
using System.Linq;
using Assent;
using FluentAssertions;
using FluentAssertions.Execution;
using HtmlAgilityPack;
using Microsoft.DotNet.Interactive.Formatting;
using Xunit;

namespace Microsoft.DotNet.Interactive.nteract.Extension.Tests
{
    public class FormatterTests : IDisposable
    {
        private readonly Configuration _configuration;

        public FormatterTests()
        {
            DataExplorerExtensions.RegisterFormatters();
            _configuration = new Configuration()
                .SetInteractive(Debugger.IsAttached);

        }

        [Fact]
        public void can_generate_tabular_json()
        {
            var data = new[]
            {
                new {Name = "Q", IsValid =false, Cost=10.0},
                new {Name = "U", IsValid =false, Cost=5.0},
                new {Name = "E", IsValid =true, Cost=10.0},
                new {Name = "S", IsValid =false, Cost=10.0},
                new {Name = "T", IsValid =false, Cost=10.0}
            };

            var formattedData = data.ToTabularData();

            
            this.Assent(formattedData.ToString(), _configuration);
        }

        [Fact]
        public void can_format_data()
        {
            var data = new[]
            {
                new {Name = "Q", IsValid =false, Cost=10.0},
                new {Name = "U", IsValid =false, Cost=5.0},
                new {Name = "E", IsValid =true, Cost=10.0},
                new {Name = "S", IsValid =false, Cost=10.0},
                new {Name = "T", IsValid =false, Cost=10.0}
            };

            var formattedData = data.ToDisplayString(TableFormatter.MimeType);


            this.Assent(formattedData, _configuration);
        }

        [Fact]
        public void can_generate_widget()
        {
            var data = new[]
            {
                new {Name = "Q", IsValid =false, Cost=10.0},
                new {Name = "U", IsValid =false, Cost=5.0},
                new {Name = "E", IsValid =true, Cost=10.0},
                new {Name = "S", IsValid =false, Cost=10.0},
                new {Name = "T", IsValid =false, Cost=10.0}
            };

            var explorer = data.Explore();

            var formatted = explorer.ToDisplayString(HtmlFormatter.MimeType);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(formatted);

            var scripts = htmlDoc.DocumentNode.SelectNodes("//div/script").Single();

            using var _ = new AssertionScope();
            scripts.InnerText.Should().Contain(data.ToTabularData().ToString());

            scripts.InnerText.Should().Contain("dotnetInteractiveExtensionsRequire(['dotnet-interactive-extensions/nteract/resources/lib'],");

        }

        public void Dispose()
        {
            Formatter.ResetToDefault();
        }
    }
}
