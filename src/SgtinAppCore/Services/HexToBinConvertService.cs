using System;
using convert = System.Convert;
using System.Linq;
using SgtinAppCore.Interfaces;

namespace SgtinAppCore.Services
{
    public class HexToBinConvertService : IHexToBinConvertService
    {
        public string Convert(string inputHex) => String.Join(
            String.Empty, 
            inputHex.Select(c => convert.ToString(convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))
        );         
    }
}
