using Moq;
using SgtinAppCore;
using SgtinAppCore.Interfaces;
using SgtinAppCore.Model;
using SgtinAppCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class SearchTagServiceTest
    {
        [Fact]
        public void SearchTagService_Found()
        {
            var repo = new Mock<IRepository<SgtinData>>();
            repo.Setup(r => r.SearchBySpec(It.IsAny<string>())).Returns(new List<SgtinData> { new SgtinData(1, "company", 11, "item") });  
            var convert = new Mock<ISgtinFactoryService>();
            convert.Setup(c => c.CreateFromHex(It.IsAny<string>())).Returns(new Sgtin(0, 0, 0, 0, 11, 0));
            var search = new SearchService(repo.Object, convert.Object);
            Assert.True(search.Compare(null, null));
        }

        [Fact]
        public void SearchTagService_NotFound()
        {
            var repo = new Mock<IRepository<SgtinData>>();
            repo.Setup(r => r.SearchBySpec(It.IsAny<string>())).Returns(new List<SgtinData> { new SgtinData(1, "company", 11, "item") });
            var convert = new Mock<ISgtinFactoryService>();
            convert.Setup(c => c.CreateFromHex(It.IsAny<string>())).Returns(new Sgtin(0, 0, 0, 0, 22, 0));
            var search = new SearchService(repo.Object, convert.Object);
            Assert.False(search.Compare(null, null));
        }
    }
}
