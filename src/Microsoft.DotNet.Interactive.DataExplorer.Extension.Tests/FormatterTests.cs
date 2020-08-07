using System;
using System.Collections;
using System.Diagnostics;
using Assent;
using FluentAssertions;
using Microsoft.DotNet.Interactive.Formatting;

using Xunit;

namespace Microsoft.DotNet.Interactive.DataExplorer.Extension.Tests
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

        public void Dispose()
        {
            Formatter.ResetToDefault();
        }
    }
}
