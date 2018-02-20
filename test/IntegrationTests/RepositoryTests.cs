using Microsoft.Extensions.Logging;
using Moq;
using SgtinInfrastructure;
using System;
using System.Linq;
using Xunit;

namespace IntegrationTests
{
    public class RepositoryTests
    {
        [Fact]
        public void TestSearchBySpec()
        {
            var repo = new SgtinDataRepository(Mock.Of<ILogger<SgtinDataRepository>>());
            var result = repo.SearchBySpec("Milka Oreo").FirstOrDefault();
            Assert.Equal((uint)69124, result.CompanyPrefix);
            Assert.Equal((uint)1253252, result.ItemReference);
        }
    }
}
