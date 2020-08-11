using System;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.nteract.Extension
{
    internal static class ExplorerExtensions
    {
        internal static HtmlString GenerateHtml(this DataExplorer dataExplorer)
        {
            var divId = Guid.NewGuid().ToString("N");
            var data = dataExplorer.Data.ToTabularData().ToString();
            var code = new StringBuilder();
            code.AppendLine("<div>");
            code.AppendLine($"<div id=\"{divId}\" style=\"height: 100ch ;margin: 2px;\">");
            code.AppendLine("</div>");
            code.AppendLine(@"<script type=""text/javascript"">
dotnetInteractiveExtensionsRequire(['dotnet-interactive-extensions/nteract/resources/lib'], (nteract) => {");
            code.AppendLine($@" let data = {data};");
            code.AppendLine($@" let viewer = nteract.createDataExplorer({{
        data: data,
        container: document.getElementById(""{divId}"")
    }});
}},
(error) => {{ 
    console.log(error); 
}});");
            code.AppendLine(" </script>");
            code.AppendLine("</div>");
            return new HtmlString(code.ToString());
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
                TableFormatter.nteractMimeType
            );

            Formatter<DataExplorer>.Register((explorer, writer) =>
            {
                var html = explorer.GenerateHtml();
                writer.Write(html);
            }, HtmlFormatter.MimeType);
        }
    }
}