using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

using RLToolkit;
using RLToolkit.Basic;
using RLToolkit.Logger;
using NUnit.Framework;

namespace RLToolkit.UnitTests.Modules
{
    [TestFixture]
    public class CommLayerTest : TestHarness, ITestBase
    {
        #region Interface Override
        public string ModuleName()
        {
            return "CommLayer";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }
        #endregion

        #region Basic
        [Test]
        public void CommLayer_UDP_SendTextValid()
        {
            List<string> listData = new List<string> { "data1", "data2", "foobar"};

            UdpReceiver r = new UdpReceiver(15000);
            UdpSender s = new UdpSender("127.0.0.1", 15000);
            r.StartListen();
            s.SendData(listData);

            ManualResetEvent mre = new ManualResetEvent(false);
            mre.WaitOne(1000);

            List<Byte[]> ret = r.GetBytesData();
            Assert.AreEqual(listData.Count, ret.Count, "We should have received the same number of items like sent.");
            for (int i = 0; i < ret.Count; i++)
            {
                Assert.AreEqual(Encoding.ASCII.GetString(ret[i]), listData[i], "Data received should be the same.");
            }

        }
        #endregion
    }
}
