using System;
using System.Collections;
using System.Text;
using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.nteract.Extension
{
    internal static class ExplorerExtensions
    {
        internal static string GenerateHtml(this nteract.DataExplorer dataExplorer)
        {
            var divId = Guid.NewGuid().ToString("N");
            var data = dataExplorer.Data.ToTabularData().ToString();
            var code = new StringBuilder();
            code.AppendLine("<div>");
            code.AppendLine($"<div id=\"{divId}\" style=\"height: 100ch ;margin: 2px;\">");
            code.AppendLine("</div>");
            code.AppendLine(@"<script type=\=""text/javascript"">
dotnetInteractiveExtensionsRequire('dotnet-interactive-extensions/dataexplorer/lib.js', (interactiveDataExplorer) => {");
            code.AppendLine($@" let data = {data};");
            code.AppendLine($@" let viewer = interactiveDataExplorer.createDataExplorer({{
        data: data,
        container: document.getElementById(""{divId}"")
    }});
}});");
            code.AppendLine(" </script>");
            code.AppendLine("</div>");
            return code.ToString();
        }
    }
    public static class DataExplorerExtensions
    {
        public static T UseDataExplorer<T>(this T kernel) where T : Kernel
        {
            RegisterFormatters();
            return kernel;
        }

        public static void RegisterFormatters()
        {
            Formatter.Register(
                typeof(IEnumerable),
                (source, writer) =>
                {
                    if (source.GetType() != typeof(string))
                    {
                        var tabularData = ((IEnumerable)source).ToTabularData();
                        writer.Write(tabularData.ToString());
                    }
                },
                TableFormatter.MimeType
            );

            Formatter<DataExplorer>.Register((explorer, writer) =>
            {
                var html = explorer.GenerateHtml();
                writer.Write(html);
            }, HtmlFormatter.MimeType);
        }
    }
}