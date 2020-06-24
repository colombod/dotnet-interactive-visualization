using System.Threading.Tasks;
using Microsoft.DotNet.Interactive.CSharp;

namespace Microsoft.DotNet.Interactive.SandDance.Extension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(Kernel kernel)
        {
            SandDanceExtensions.RegisterFormatters();

            switch (kernel)
            {
                case CSharpKernel cSharpKernel:
                    cSharpKernel.UseSandDance();
                    break;
            }

            return Task.CompletedTask;
        }
    }
}