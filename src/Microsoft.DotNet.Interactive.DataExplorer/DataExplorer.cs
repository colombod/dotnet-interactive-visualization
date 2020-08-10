using System.Collections;

namespace Microsoft.DotNet.Interactive.nteract
{
    public class DataExplorer
    {
        public IEnumerable Data { get; }

        internal DataExplorer(IEnumerable data)
        {
            Data = data;
        }
    }
}