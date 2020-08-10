using System.Threading.Tasks;

namespace Microsoft.DotNet.Interactive.nteract.Extension
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
