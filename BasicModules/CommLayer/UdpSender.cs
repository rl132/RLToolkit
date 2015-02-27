using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

using RLToolkit.Logger;

// TODO: RL: Add better return information

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
            this.port = port;
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            address = IPAddress.Parse(ip);
            senderEP = new IPEndPoint(address, port);
            isReady = true;
        }

        public void SendData(IEnumerable<string> input)
        {
            if (!isReady)
            {
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
    }
}

