using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

using RLToolkit.Logger;

namespace RLToolkit.Basic
{
    public class UdpSender
    {
        private int port = 11000;
        private IPAddress address;
        private Socket sender;
        private IPEndPoint senderEP;
        private bool isReady = false;

        public UdpSender(string ip, int port)
        {
            this.Log().Debug(string.Format("Trying to open a UDP Sender to IP: {0} on port: {1}", ip, port.ToString()));
            this.port = port;
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            address = IPAddress.Parse(ip);
            senderEP = new IPEndPoint(address, port);
            isReady = true;
        }

        public void SendData(IEnumerable<string> input)
        {
            this.Log().Debug("Trying to send an array of string through UDP");     
            if (!isReady)
            {
                this.Log().Warn("UDP Sender not initialized.");
                return;
            }

            foreach (string str in input)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);

                try
                {
                    sender.SendTo(buffer, senderEP);                                                
                }
                catch (Exception e)
                {
                    this.Log().Warn("Caught Exception:\n" + e.ToString());
                    // fubar!
                }
            }
        }

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
    }
}

