using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.nteract.Extension
{
    public static class DataExplorerExtensions
    {
        public static T UseDataExplorer<T>(this T kernel) where T : Kernel
        {
            RegisterFormatters();
            return kernel;
        }

        public static void RegisterFormatters()
        {
            Formatter.Register<IEnumerable>(
                (_, source, writer) =>
                {
                    if (source.GetType() != typeof(string))
                    {
                        var tabularData = source.ToTabularData();
                        writer.Write(tabularData.ToString());
                        return true;
                    }

                    return false;
                },
                TableFormatter.MimeType
            );

            Formatter.Register(
                typeof(IEnumerable),
                (source, writer) =>
                {
                    if (source.GetType() != typeof(string))
                    {
                        var tabularData = ((IEnumerable) source).ToTabularData();
                        writer.Write(tabularData.ToString());
                    }
                },
                TableFormatter.nteractMimeType
            );

            Formatter.Register<TabularJsonString>((explorer, writer) =>
            {
                var html = explorer.RenderDataExplorer();
                writer.Write(html);
            }, HtmlFormatter.MimeType);

            // this is a formatter for SQL data
            Formatter.Register
            <IEnumerable /* tables*/
                <IEnumerable /* rows */
                    <IEnumerable /* fields */<(string, object)>>>>((source, writer) =>
            {
                // TODO-JOSEQU: (RegisterFormatters) do all the tables...

                writer.Write(source.First()
                                   .ToTabularData()
                                   .ToDisplayString("text/html"));
            }, "text/html");
        }

        internal static HtmlString RenderDataExplorer(this TabularJsonString data)
        {
            var divId = Guid.NewGuid().ToString("N");
            var code = new StringBuilder();
            code.AppendLine("<div>");
            code.AppendLine($"<div id=\"{divId}\" style=\"height: 100ch ;margin: 2px;\">");
            code.AppendLine("</div>");
            code.AppendLine(@"<script type=""text/javascript"">
getExtensionRequire('nteract','1.0.0')(['nteract/lib'], (nteract) => {");
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
}