using System.Collections;

namespace Microsoft.DotNet.Interactive.DataExplorer
{
    public class Explorer
    {
        public IEnumerable Source { get; }

        internal Explorer(IEnumerable source)
        {
            Source = source;
        }
    }
}