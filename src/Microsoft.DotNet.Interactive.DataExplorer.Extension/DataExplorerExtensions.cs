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
            throw new System.NotImplementedException();
        }
    }
}