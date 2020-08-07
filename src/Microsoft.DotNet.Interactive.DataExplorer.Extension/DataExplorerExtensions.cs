using System.Collections;
using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.DataExplorer.Extension
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
            Formatter<IEnumerable>.Register((enumerable, writer) =>
           {
               var tabularData = enumerable.ToTabularData();

               writer.Write(tabularData.ToString(formatting: Newtonsoft.Json.Formatting.Indented));
           }, TableFormatter.MimeType);
        }
    }
}