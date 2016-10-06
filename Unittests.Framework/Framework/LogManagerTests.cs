using System.IO;
using CH9.Framework.Logging;
using NUnit.Framework;

namespace Unittests.Framework.Framework
{
    [TestFixture]
    public class LogManagerTests
    {
        private const string Infolog = @"c:\temp\info.log";
        private const string Debuglog = @"c:\temp\debug.log";
        [TestCase(LogLevel.Info)]
        [TestCase(LogLevel.Debug)]
        public void TestBasicMessageSendStringParams(LogLevel lgoLevel)
        {
            // arrange
            var mf = "Test{0}";
            var p = 2;
            var msg = string.Format(mf, p);
            

            // Act
            Ch9LogManager.Log(GetType(), lgoLevel, msg, p);

            //Assert
            Assert.IsTrue(lgoLevel == LogLevel.Info? File.Exists(Infolog) : File.Exists(Debuglog));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(Infolog))
            {
                File.Delete(Infolog);
            }

            if (File.Exists(Debuglog))
            {
                File.Delete(Debuglog);
            }
        }
    }
}
