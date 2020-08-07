using System.Threading.Tasks;
using Microsoft.DotNet.Interactive.CSharp;

namespace Microsoft.DotNet.Interactive.DataExplorer.Extension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(Kernel kernel)
        {
            kernel.UseDataExplorer();
            return Task.CompletedTask;
        }
    }
}
