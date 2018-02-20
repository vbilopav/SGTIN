using SgtinAppCore;
using SgtinAppCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class SgtinFactoryServiceTest
    {
        [Fact]
        public void TestSgtinFactoryService_CreateFromHexFail_Malformed()
        {
            var factory = new SgtinFactoryService(new BinToIntConvertService(), new HexToBinConvertService());            
            Assert.Throws<SgtinException>(() => factory.CreateFromHex("incorrent input"));
        }

        [Fact]
        public void TestSgtinFactoryService_CreateFromHexFail_TooShort()
        {
            var factory = new SgtinFactoryService(new BinToIntConvertService(), new HexToBinConvertService());
            Assert.Throws<SgtinException>(() => factory.CreateFromHex("309B60F9A167850019EA8A4"));
        }

        [Fact]
        public void TestSgtinFactoryService_CreateFromHexFail_TooLong()
        {
            var factory = new SgtinFactoryService(new BinToIntConvertService(), new HexToBinConvertService());
            Assert.Throws<SgtinException>(() => factory.CreateFromHex("309B60F9A167850019EA8A4E1"));
        }

        [Fact]
        public void TestSgtinFactoryService_CreateFromHexFail_WrongHeader()
        {
            var factory = new SgtinFactoryService(new BinToIntConvertService(), new HexToBinConvertService());
            Assert.Throws<SgtinException>(() => factory.CreateFromHex("319B60F9A167850019EA8A4E1"));
        }
        
        [Fact]
        public void TestSgtinFactoryService_CreateFromHexSuccessuful()
        {
            var factory = new SgtinFactoryService(new BinToIntConvertService(), new HexToBinConvertService());
            var result = factory.CreateFromHex("3074257BF7194E4000001A85");
            Assert.Equal(0x30, result.Header);
            Assert.Equal(0b_011, result.Filter);
            Assert.Equal(5, result.Partition);
            Assert.Equal((ulong)0b_0000_1001_0101_1110_1111_1101, result.Gs1CompanyPrefix);
            Assert.Equal((ulong)0b_1100_0110_0101_0011_1001, result.ItemReference);
            Assert.Equal((ulong)0b_0000_0000_0000_0000_0000_0000_0110_1010_0001_01, result.SerialReference);
        }
    }
}
