using SgtinAppCore;
using SgtinAppCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class ConvertServiceTests
    {
        [Theory]
        [InlineData("A", "1010")]
        [InlineData("AB", "10101011")]
        [InlineData("ABC", "101010111100")]
        [InlineData("123", "000100100011")]
        [InlineData("ABC123", "101010111100000100100011")]
        public void TestHexToBinConvrter_Converts(string input, string expected)
        {
            var c = new HexToBinConvertService();
            Assert.Equal(c.Convert(input), expected);
        }

        [Fact]
        public void TestHexToBinConvrter_ConvertFails()
        {
            var c = new HexToBinConvertService();
            Assert.Throws<FormatException>(() => c.Convert("invalid hex"));
        }


        [Theory]
        [InlineData("1010", 0b_1010)]
        [InlineData("10101010", 0b_1010_1010)]
        [InlineData("1111", 0b_1111)]
        [InlineData("111100", 0b_1111_00)]
        public void TestBinToIntConvrter_ConvertsToByte(string input, byte expected)
        {
            var c = new BinToIntConvertService();
            var r = c.ToByte(input);
            Assert.IsType<byte>(r);
            Assert.Equal(expected, r);
        }

        [Theory]
        [InlineData("1010", 0b_1010)]
        [InlineData("10101010", 0b_1010_1010)]
        [InlineData("1010101011111010", 0b_1010_1010_1111_1010)]
        [InlineData("1010101011111010111100001111000010101111111100001010101011001100", 0b_1010_1010_1111_1010_1111_0000_1111_0000_1010_1111_1111_0000_1010_1010_1100_1100)]
        public void TestBinToIntConvrter_ConvertsToLong(string input, ulong expected)
        {
            var c = new BinToIntConvertService();
            var r = c.ToLong(input);
            Assert.IsType<ulong>(r);
            Assert.Equal(expected, r);
        }

        [Fact]
        public void TestBinToIntConvrter_ConvertFails()
        {
            var c = new BinToIntConvertService();
            Assert.Throws<FormatException>(() => c.ToByte("invalid bin"));
        }
    }
}
