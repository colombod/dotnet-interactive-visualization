using System.Threading.Tasks;
using Microsoft.DotNet.Interactive.CSharp;

namespace Microsoft.DotNet.Interactive.SandDance.Extension
{
    public class KernelExtension : IKernelExtension, IStaticContentSource
    {
        public Task OnLoadAsync(Kernel kernel)
        {
            switch (kernel)
            {
                case CSharpKernel cSharpKernel:
                    cSharpKernel.UseSandDance();
                    break;
            }

            return Task.CompletedTask;
        }

        public string Name => "sanddance";
    }
}