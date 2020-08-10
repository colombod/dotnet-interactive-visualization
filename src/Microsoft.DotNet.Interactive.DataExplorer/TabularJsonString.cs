using Microsoft.DotNet.Interactive.Formatting;

namespace Microsoft.DotNet.Interactive.nteract
{
    public class TabularJsonString : JsonString
    {
        public TabularJsonString(string json)
            : base(json)
        {
        }
    }
}