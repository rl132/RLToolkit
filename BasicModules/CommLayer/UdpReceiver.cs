using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;

using RLToolkit.Logger;

// TODO : RL: Add some better logging & return information if we hit an odd
//        state like trying to listen while not being initialized.

namespace RLToolkit.Basic
{
    public class UdpReceiver
    {
        private int port = 11000;
        private UdpClient listener;
        private IPEndPoint groupEP;
        private List<Byte[]> receivedBytes = new List<byte[]>();

        private static Thread threadListen;

        private bool isToClear = false;
        private bool isReady = false;
        private bool isListening = false;

        public UdpReceiver(int port)
        {
            this.port = port;
            listener = new UdpClient(port);
            groupEP = new IPEndPoint(IPAddress.Any, port);
            isReady = true;
        }

        public void StartListen()
        {
            if (!isReady)
            {
                // not initialized
                return;
            }

            if (isListening)
            {
                // already listening
                return;
            }

            threadListen = new Thread(tick);
            threadListen.Start();
            isListening = true;
        }

        public void StopListen()
        {
            if (!isReady)
            {
                // not initialized
                return;
            }

            if (!isListening)
            {
                // not listening
                return;
            }

            threadListen.Abort();
            threadListen = null;
        }

        public void CloseConnection()
        {
            if (!isReady)
            {
                // not initialized
                return;
            }

            if (isListening)
            {
                StopListen();
            }

            // close the connection
            listener.Close();
        }

        public List<byte[]> GetBytesData()
        {
            if (isToClear)
            {
                // we're to be cleared anywyas
                return new List<byte[]>();
            }

            List<Byte[]> output = new List<byte[]>(receivedBytes);
            isToClear = true;

            return output;
        }

        private void tick()
        {
            // execute on a separate thread.
            while (isListening)
            {
                try
                {
                    // will wait until we have something received
                    byte[] bytesIn = listener.Receive(ref groupEP);

                    if (isToClear)
                    {
                        // clear the list before adding anything new
                        receivedBytes = new List<byte[]>();
                        isToClear = false;
                    }

                    receivedBytes.Add(bytesIn);
                } 
                catch (Exception e)
                {
                    this.Log().Warn("Caught Exception:\n" + e.ToString());
                    // fubar
                }

                // sleep
                Thread.Sleep(25);
            }
        }
    }
}

