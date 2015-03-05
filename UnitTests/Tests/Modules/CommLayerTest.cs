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

        #region UDP Basic
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
            r.CloseConnection();
            s.CloseConnection();
        }

        [Test]
        public void CommLayer_UDP_SendBytesValid()
        {
            List<string> listDataIn = new List<string> { "data1", "data2", "foobar"};
            List<byte[]> listData = new List<byte[]>();
            foreach (string str in listDataIn)
            {
                listData.Add(Encoding.ASCII.GetBytes(str));
            }

            UdpReceiver r = new UdpReceiver(15000);
            UdpSender s = new UdpSender("127.0.0.1", 15000);
            r.StartListen();
            s.SendData(listData);

            ManualResetEvent mre = new ManualResetEvent(false);
            mre.WaitOne(1000);

            List<Byte[]> ret = r.GetBytesData();
            Assert.AreEqual(listDataIn.Count, ret.Count, "We should have received the same number of items like sent.");
            for (int i = 0; i < ret.Count; i++)
            {
                Assert.AreEqual(Encoding.ASCII.GetString(ret[i]), listDataIn[i], "Data received should be the same.");
            }
            r.CloseConnection();
            s.CloseConnection();
        }

        [Test]
        public void CommLayer_UDP_SendWithDelay()
        {
            List<string> listData = new List<string> { "data1", "data2", "foobar"};
            List<string> listData2 = new List<string> { "data3", "dataX", "bloop"};

            UdpSender s = new UdpSender("127.0.0.1", 15000);
            s.SendData(listData);

            ManualResetEvent mre = new ManualResetEvent(false);
            mre.WaitOne(1000);

            UdpReceiver r = new UdpReceiver(15000);
            s.SendData(listData2);
            r.StartListen();
            mre.WaitOne(1000);

            List<Byte[]> ret = r.GetBytesData();
            Assert.AreEqual(listData2.Count, ret.Count, "We should have received the same number of items like sent the second time.");
            for (int i = 0; i < ret.Count; i++)
            {
                Assert.AreEqual(Encoding.ASCII.GetString(ret[i]), listData2[i], "Data received should be the same.");
            }
            r.CloseConnection();
            s.CloseConnection();
        }
        #endregion

        #region UDP hardness tests
        [Test]
        public void CommLayer_UDP_BadIPFormat()
        {
            UdpSender s;
            try
            {
                s = new UdpSender("127.6666.s.4536ss", 15000);
                s.CloseConnection();
            }
            catch (FormatException e)
            {
                this.Log().Debug("Expected exception" + Environment.NewLine + e.ToString());
                // expected
            }

        }

        [Test]
        public void CommLayer_UDP_WrongPort()
        {
            List<string> listData = new List<string> { "data1", "data2", "foobar"};

            UdpReceiver r = new UdpReceiver(15000);
            UdpSender s = new UdpSender("127.0.0.1", 14999);
            r.StartListen();
            s.SendData(listData);

            ManualResetEvent mre = new ManualResetEvent(false);
            mre.WaitOne(1000);

            List<Byte[]> ret = r.GetBytesData();
            Assert.AreEqual(0, ret.Count, "We should have received nothing.");
            r.CloseConnection();
            s.CloseConnection();
        }
        #endregion

        #region UDP Get calls
        [Test]
        public void CommLayer_UDP_GetSender()
        {
            UdpSender s = new UdpSender("127.0.0.1", 16000);
            Assert.AreEqual(16000, s.GetPort(), "Sender ports should return 16000");
            s.CloseConnection();
        }

        [Test]
        public void CommLayer_UDP_GetReceiver()
        {
            UdpReceiver r = new UdpReceiver(17000);
            Assert.AreEqual(17000, r.GetPort(), "Receiver ports should return 17000");
            r.CloseConnection();
        }
        #endregion
    }
}
