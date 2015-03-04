using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;

using RLToolkit.Logger;

namespace RLToolkit.Basic
{
    /// <summary>
    /// UDP receiver.
    /// </summary>
    public class UdpReceiver
    {
        #region Global variables
        private int port = 11000;
        private UdpClient listener;
        private IPEndPoint groupEP;
        private List<Byte[]> receivedBytes = new List<byte[]>();
        private static Thread threadListen;
        private bool isToClear = false;
        private bool isReady = false;
        private bool isListening = false;

        /// <summary>
        /// Flag to query in order to know if there's new data available in the buffer.
        /// </summary>
        public bool isDataAvailable = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the class with a port specified. It will also initialize the Receiver but not start listening.
        /// </summary>
        /// <param name="port">The port on which to listen</param>
        public UdpReceiver(int port)
        {
            this.Log().Debug(string.Format("Trying to open a UDP Receiver on port: {0}", port.ToString()));
            this.port = port;
            listener = new UdpClient(port);
            groupEP = new IPEndPoint(IPAddress.Any, port);
            isReady = true;
        }
        #endregion

        #region State control Methods
        /// <summary>
        /// Method to start the Listening
        /// </summary>
        public void StartListen()
        {
            this.Log().Debug("Trying to start listening on the UDP Receiver");
            if (!isReady)
            {
                this.Log().Warn("UDP Receiver not initialized.");
                // not initialized
                return;
            }

            if (isListening)
            {
                this.Log().Warn("UDP Receiver already listening.");
                // already listening
                return;
            }

            threadListen = new Thread(tick);
            threadListen.Start();
            isListening = true;
        }

        /// <summary>
        /// Method to Stop the listening
        /// </summary>
        public void StopListen()
        {
            this.Log().Debug("Trying to stop listening on the UDP Receiver");
            if (!isReady)
            {
                this.Log().Warn("UDP Receiver not initialized.");
                // not initialized
                return;
            }

            if (!isListening)
            {
                this.Log().Warn("UDP Receiver not listening.");
                // not listening
                return;
            }

            threadListen.Abort();
            threadListen = null;
        }

        /// <summary>
        /// Method to close the connection. Will autoStop listening if it was.
        /// </summary>
        public void CloseConnection()
        {
            this.Log().Debug("Trying to close connection on the UDP Receiver");
            if (!isReady)
            {
                this.Log().Warn("UDP Receiver not initialized.");
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

        /// <summary>
        /// Method to fetch the bytes received by the listener.
        /// </summary>
        /// <returns>a List of Byte Array containing all the messagesreceived.</returns>
        public List<byte[]> GetBytesData()
        {
            this.Log().Debug("Trying to fetch Receiver data");
            if (isToClear)
            {
                this.Log().Warn("Trying to fetch an empty byte buffer.");
                // we're to be cleared anywyas
                return new List<byte[]>();
            }

            List<Byte[]> output = new List<byte[]>(receivedBytes);
            isDataAvailable = false;
            isToClear = true;

            return output;
        }
        #endregion

        #region private methods
        private void tick()
        {
            this.Log().Debug("Starting the listen tick");
            // execute on a separate thread.
            while (isListening)
            {
                try
                {
                    // will wait until we have something received
                    byte[] bytesIn = listener.Receive(ref groupEP);
                    this.Log().Debug("Got some data");

                    if (isToClear)
                    {
                        // clear the list before adding anything new
                        receivedBytes = new List<byte[]>();
                        isToClear = false;
                    }

                    receivedBytes.Add(bytesIn);
                    isDataAvailable = true;
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
        #endregion
    }
}