using System;
using System.Collections.Generic;
using System.Text;

namespace SgtinAppCore.Model
{
    public class Sgtin
    {
        public byte Header { get; }
        public byte Filter { get; }
        public byte Partition { get; }
        public ulong Gs1CompanyPrefix { get; }
        public ulong ItemReference { get; }
        public ulong SerialReference { get; }

        public Sgtin(byte header, byte filter, byte partition, ulong gs1CompanyPrefix, ulong itemReference, ulong serialReference)
        {
            Header = header;
            Filter = filter;
            Partition = partition;
            Gs1CompanyPrefix = gs1CompanyPrefix;
            ItemReference = itemReference;
            SerialReference = serialReference;
        }
    }
}
