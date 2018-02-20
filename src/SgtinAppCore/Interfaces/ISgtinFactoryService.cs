using SgtinAppCore.Model;

namespace SgtinAppCore.Interfaces
{
    public interface ISgtinFactoryService
    {
        Sgtin CreateFromHex(string inputHex);
    }
}