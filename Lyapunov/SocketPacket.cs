using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Lyapunov
{
    class SocketPacket
    {
        public SocketPacket(Socket soc)
        {
            Socket = soc;
        }
        public Socket Socket;
        public byte[] dataBuffer = new byte[4096];
    }
}
