using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

using RLToolkit.Logger;

namespace RLToolkit.Basic
{
    /// <summary>
    /// UDP sender.
    /// </summary>
    public class UdpSender
    {
        #region Global variables
        private int port = 11000;
        private IPAddress address;
        private Socket sender;
        private IPEndPoint senderEP;
        private bool isReady = false;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor that initialize a sender with an IP and a Port. 
        /// </summary>
        /// <param name="ip">Ip Adress to use in string form</param>
        /// <param name="port">The port to use</param>
        public UdpSender(string ip, int port)
        {
            this.Log().Debug(string.Format("Trying to open a UDP Sender to IP: {0} on port: {1}", ip, port.ToString()));
            this.port = port;
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            address = IPAddress.Parse(ip);
            senderEP = new IPEndPoint(address, port);
            isReady = true;
        }
        #endregion

        #region Sender methods
        /// <summary>
        /// Method to send an IEnumerable array of string
        /// </summary>
        /// <param name="input">Data as string</param>
        public void SendData(IEnumerable<string> input)
        {
            this.Log().Debug("Trying to send an array of string through UDP");     

            foreach (string str in input)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                SendData(buffer);
            }
        }

        /// <summary>
        /// Method to send an IEnumerable array of byte array
        /// </summary>
        /// <param name="input">Data as byte array</param>
        public void SendData(IEnumerable<byte[]> input)
        {
            this.Log().Debug("Trying to send an array of bytes through UDP");     

            foreach (byte[] b in input)
            {
                SendData(b);
            }
        }

        /// <summary>
        /// Method to send byte array 
        /// </summary>
        /// <param name="input">the byte array to send</param>
        public void SendData(byte[] input)
        {
            this.Log().Debug("Trying to send a byte array through UDP");
            if (!isReady)
            {
                this.Log().Warn("UDP Sender not initialized.");
                return;
            }

            try
            {
                sender.SendTo(input, senderEP);
            }
            catch (Exception e)
            {
                this.Log().Warn("Caught Exception:\n" + e.ToString());
                // fubar!
            }
        }
        #endregion
    }
}