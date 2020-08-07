// formatter that implements the tabular mimetype as described at https://specs.frictionlessdata.io/table-schema/

namespace Microsoft.DotNet.Interactive.DataExplorer
{
    public static class TableFormatter
    {
        public static string MimeType => "application/table-schema+json";
    }
}
