///////////////////////////////////////////////////////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmiBrokerPlugin.Tests
{
    [TestClass]
    public class PluginTest
    {
        [TestMethod]
        public void GetPluginInfo_Should_Return_Plugin_Info()
        {
            // Arrange
            var pluginInfo = new PluginInfo();

            // Act
            var result = Plugin.GetPluginInfo(ref pluginInfo);

            // Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual("ShubhaLabha2 ® data Plug-in", pluginInfo.Name);
            Assert.AreEqual("ShubhaRt", pluginInfo.Vendor);
            Assert.AreEqual(1095584080, pluginInfo.IDCode);
            Assert.AreEqual(PluginType.Data, pluginInfo.Type);
            Assert.AreEqual(10000, pluginInfo.Version);
            Assert.AreEqual(0, pluginInfo.Certificate);
            Assert.AreEqual(387000, pluginInfo.MinAmiVersion);
        }

        [TestMethod]
        public void Init_Test()
        {
            // Act
            var result = Plugin.Init();

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Release_Test()
        {
            // Act
            var result = Plugin.Release();

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        unsafe public void GetQuotesEx_Test()
        {
            // Arrange
            var quotes = stackalloc Quotation[250];
            var context = stackalloc GQEContext[1];

            // Act
            var result = Plugin.GetQuotesEx("TEST", Periodicity.EndOfDay, 1, 250, quotes, context);
            
            // Assert
            Assert.IsTrue(result > 0);
        }
    }
}
