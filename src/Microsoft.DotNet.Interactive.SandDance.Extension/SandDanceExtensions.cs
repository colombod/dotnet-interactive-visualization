using System;
using System.IO;
using System.Text;

using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.SandDance.Extension
{
    public static class SandDanceExtensions 
    {

        public static void RegisterFormatters()
        {
            Formatter<DataExplorer>.Register((explorer, writer) =>
            {
                var html = GenerateHtml(explorer);
                writer.Write(html);
            }, HtmlFormatter.MimeType);
        }

        private static string GenerateHtml(DataExplorer explorer)
        {
            var divId = Guid.NewGuid().ToString("N");
            var data = explorer.GetData().SerializeToJson();
            var code = new StringBuilder();
            code.AppendLine("<div>");
            code.AppendLine($"<div id=\"{divId}\" style=\"height: 100ch ;margin: 2px;\">");
            code.AppendLine("</div>");
            code.AppendLine(@"<script type=""text/javascript"">
dotnetInteractiveExtensionsRequire('dotnet-interactive-extensions/sanddance/lib', (interactiveSandDance) => {");
            code.AppendLine($@" let viewer = interactiveSandDance.createSandDance(document.getElementById(""{divId}""), ""explorer"");");
            code.AppendLine($@" viewer.loadData({data});
}});");
            code.AppendLine(" </script>");

            code.AppendLine("</div>");


            return code.ToString();
        }

        public static T UseSandDance<T>(this T kernel) where T : Kernel
        {
            RegisterFormatters();
            return kernel;
        }
    }
}