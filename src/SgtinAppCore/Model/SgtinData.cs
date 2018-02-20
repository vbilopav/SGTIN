using System;
using System.Collections.Generic;
using System.Text;

namespace SgtinAppCore.Model
{
    public class SgtinData
    {        
        public ulong CompanyPrefix { get; }
        public string CompanyName { get; }
        public ulong ItemReference { get; }
        public string ItemName { get; }

        public SgtinData(ulong companyPrefix, string companyName, ulong itemReference, string itemName)
        {
            CompanyPrefix = companyPrefix;
            CompanyName = companyName;
            ItemReference = itemReference;
            ItemName = itemName;
        }
    }
}
