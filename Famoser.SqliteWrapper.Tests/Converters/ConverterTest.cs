using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Converters;
using Famoser.SqliteWrapper.Converters.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.SqliteWrapper.Tests.Converters
{
    [TestClass]
    public class ConverterTest
    {
        [TestMethod]
        public void TestGuidStringConverter()
        {
            var str = "21d3dc17-8f63-4624-8b5b-9ba985c5091f";
            var guid = Guid.Parse(str);
            var converter = new GuidStringConverter();

            Test(converter, guid, str);
        }

        [TestMethod]
        public void TestUriStringConverter()
        {
            var str = "https://www.youtube.com/watch?v=P_SlAzsXa7E";
            var guid = new Uri(str);
            var converter = new UriStringConverter();

            Test(converter, guid, str);
        }

        private void Test(IEntityMappingConverter converter, object highLevelValue, object lowLevelValue)
        {
            Assert.IsTrue(highLevelValue.Equals(converter.ConvertToModelFormat(lowLevelValue)));
            Assert.IsTrue(lowLevelValue.Equals(converter.ConvertToEntityFormat(highLevelValue)));
        }
    }
}
