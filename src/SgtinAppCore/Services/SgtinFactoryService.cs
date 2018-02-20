using System;
using System.Linq;
using SgtinAppCore.Interfaces;
using SgtinAppCore.Model;
using System.Collections.Generic;
using System.Collections;

namespace SgtinAppCore.Services
{
    public class SgtinFactoryService : ISgtinFactoryService
    {
        private const byte sgtin96 = 0x30;
        private readonly IDictionary<int, Tuple<int, int>> partitionTable = new Dictionary<int, Tuple<int, int>>
        {
            {0, Tuple.Create(40, 4) },
            {1, Tuple.Create(37, 7) },
            {2, Tuple.Create(34, 10) },
            {3, Tuple.Create(30, 14) },
            {4, Tuple.Create(27, 17) },
            {5, Tuple.Create(24, 20) },
            {6, Tuple.Create(20, 24) }
        };
        private readonly IBinToIntConvertService binToInt;
        private readonly IHexToBinConvertService hexToBin;

        private T TryConvert<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (FormatException exception)
            {
                throw new SgtinException("Error while converting binary value.", exception);
            }
        }

        public Sgtin CreateFromHex(string inputHex)
        {
            if (string.IsNullOrWhiteSpace(inputHex))
            {
                throw new SgtinException("Input hex string cannot be empty or null.");
            }
            if (inputHex.Length != 24)
            {
                throw new SgtinException("Input hex string is invalid format.");
            }

            string inputBin;
            try
            {
                inputBin = hexToBin.Convert(inputHex);
            }
            catch (FormatException exception)
            {
                throw new SgtinException("Input hex string is invalid format.", exception);
            }

            var header = TryConvert(() => binToInt.ToByte(inputBin.Substring(0, 8)));
            if (header != sgtin96)
            {
                throw new SgtinException("Only SGTIN96 formats are supported");
            }

            var filter = TryConvert(() => binToInt.ToByte(inputBin.Substring(8, 3)));
            var partition = TryConvert(() => binToInt.ToByte(inputBin.Substring(11, 3)));
            if (partition > 6)
            {
                throw new SgtinException("Invalid partition value.");
            }
            var companyPrefixBits = partitionTable[partition].Item1;
            var itemReferenceBits = partitionTable[partition].Item2;

            var gs1CompanyPrefix = TryConvert(() => binToInt.ToLong(inputBin.Substring(14, companyPrefixBits)));
            var itemReference = TryConvert(() => binToInt.ToLong(inputBin.Substring(14 + companyPrefixBits, itemReferenceBits)));
            var serialReference = TryConvert(() => binToInt.ToLong(inputBin.Substring(14 + companyPrefixBits + itemReferenceBits)));

            return new Sgtin(header, filter, partition, gs1CompanyPrefix, itemReference, serialReference);
        }

        public SgtinFactoryService(IBinToIntConvertService binToInt, IHexToBinConvertService hexToBin)
        {
            this.binToInt = binToInt;
            this.hexToBin = hexToBin;
        }
    }
}
