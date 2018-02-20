using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SgtinAppCore.Interfaces;
using SgtinAppCore.Model;

namespace SgtinAppCore.Services
{
    public class SearchService : ISearchService
    {
        private readonly IRepository<SgtinData> repository;
        private readonly ISgtinFactoryService factoryService;

        public bool Compare(string inputHex, string name)
        {
            var tag = repository.SearchBySpec(name).FirstOrDefault();
            var item = factoryService.CreateFromHex(inputHex);
            if (tag == default(SgtinData))
            {
                return false;
            }
            if (item.ItemReference == tag.ItemReference)
            {
                return true;
            }
            return false;
        }

        public SearchService(IRepository<SgtinData> repository, ISgtinFactoryService factoryService)
        {
            this.repository = repository;
            this.factoryService = factoryService;
        }
    }
}
