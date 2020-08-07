using System;
using Microsoft.DotNet.Interactive.Formatting;

using Xunit;

namespace Microsoft.DotNet.Interactive.DataExplorer.Extension.Tests
{
    public class FormatterTests : IDisposable
    {
        public FormatterTests()
        {
            DataExplorerExtensions.RegisterFormatters();
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

            var formattedData = data.ToDisplayString(TableFormatter.MimeType);
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Formatter.ResetToDefault();
        }
    }
}
