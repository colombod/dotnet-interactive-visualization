using System;
using System.Collections;
using System.IO;
using System.Text;

using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.DataExplorer.Extension
{
    internal static class ExplorerExtensions
    {
        private static readonly string _libCode;

        static ExplorerExtensions()
        {
            var assembly = typeof(ExplorerExtensions).Assembly;
            using var resourceStream = assembly.GetManifestResourceStream($"{typeof(ExplorerExtensions).Namespace}.resources.lib.js");
            if (resourceStream != null)
            {
                using var textStream = new StreamReader(resourceStream);
                _libCode = textStream.ReadToEnd();
            }
        }

        internal static string GenerateHtml(this Explorer explorer)
        {
            var divId = Guid.NewGuid().ToString("N");
            var data = explorer.Source.ToTabularData().ToString(Newtonsoft.Json.Formatting.None);
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
            Formatter<IEnumerable>.Register((enumerable, writer) =>
           {
               var tabularData = enumerable.ToTabularData();

               writer.Write(tabularData.ToString(formatting: Newtonsoft.Json.Formatting.Indented));
           }, TableFormatter.MimeType);

            Formatter<Explorer>.Register((explorer, writer) =>
            {
                var html = explorer.GenerateHtml();
                writer.Write(html);
            }, HtmlFormatter.MimeType);
        }
    }
}