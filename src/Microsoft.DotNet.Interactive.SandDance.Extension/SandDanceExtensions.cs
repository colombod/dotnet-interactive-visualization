using System;
using System.IO;
using System.Text;

using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.SandDance.Extension
{
    public static class SandDanceExtensions
    {
        private static readonly string _sandDanceCode;

        static SandDanceExtensions()
        {
            var assembly = typeof(SandDanceExtensions).Assembly;
            using var resourceStream = assembly.GetManifestResourceStream($"{typeof(SandDanceExtensions).Namespace}.resources.lib.js");
            if (resourceStream != null)
            {
                using var textStream = new StreamReader(resourceStream);
                _sandDanceCode = textStream.ReadToEnd();
            }
        }
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
            code.AppendLine("<script type=\"text/javascript\">");
            code.AppendLine(_sandDanceCode);
            code.AppendLine("</script>");

            code.AppendLine("<script type=\"text/javascript\">");
            code.AppendLine($@"let viewer = createSandDance(document.getElementById(""{divId}""), ""explorer"");");
            code.AppendLine($@"viewer.loadData({data});");
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