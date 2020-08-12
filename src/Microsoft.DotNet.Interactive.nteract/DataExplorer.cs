using System.Collections;

namespace Microsoft.DotNet.Interactive.nteract
{
    public class DataExplorer
    {
        public IEnumerable Data { get; }

        public DataExplorer(IEnumerable data)
        {
            Data = data;
        }
    }
}