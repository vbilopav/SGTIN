using convert = System.Convert;
using SgtinAppCore.Interfaces;

namespace SgtinAppCore.Services
{
    public class BinToIntConvertService : IBinToIntConvertService
    {
        public ulong ToLong(string inputBin) => convert.ToUInt64(inputBin, 2);
        public byte ToByte(string inputBin) => convert.ToByte(inputBin, 2);
    }
}
