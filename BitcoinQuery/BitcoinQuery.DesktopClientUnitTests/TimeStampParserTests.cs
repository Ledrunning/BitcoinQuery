using System;
using BitcoinQuery.DesktopClient.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitcoinQuery.DesktopClientUnitTests
{
    [TestClass]
    public class TimeStampParserTests
    {
        [TestMethod]
        public void UnixTimeStampToDateTime_ValidTimeStamp_ReturnsCorrectDateTime()
        {
            // Arrange
            long timeStamp = 1645699200;
            var expectedDateTimeLocal = new DateTime(2022, 2, 24, 11, 40, 0, DateTimeKind.Local);

            // Act
            var actualDateTime = timeStamp.UnixTimeStampToDateTime();

            // Assert
            Assert.AreEqual(expectedDateTimeLocal, actualDateTime);
        }
    }
}