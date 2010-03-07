using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Lyapunov
{
    public partial class GridForm : Form
    {
        //Socket _socklistner;
        //public AsyncCallback pfnWorkerCallBack;
        //private Socket m_mainSocket;
        ////private System.Collections.ArrayList m_workerSocketList = ArrayList.Synchronized(new System.Collections.ArrayList());
        //private int m_clientCount = 0;
        IPAddress _server;

        public GridForm()
        {
            InitializeComponent();
            //try
            //{
            //    m_mainSocket = new Socket(AddressFamily.InterNetwork,
            //        SocketType.Stream,
            //        ProtocolType.Tcp);
            //    IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
            //    m_mainSocket.Bind(ipLocal);
            //    m_mainSocket.Listen(4);
            //    m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            //}
            //catch (SocketException se)
            //{
            //    MessageBox.Show(se.Message);
            //}
        }

        private void search_btn_Click(object sender, EventArgs e)
        {

        }

        private void connect_btn_Click(object sender, EventArgs e)
        {
            _server = IPAddress.Parse(textBox1.Text);
            //byte[] msg = System.Text.Encoding.ASCII.GetBytes("hello there");
            Socket socksender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socksender.Connect(_server, 2000);
            LyapunovGenerator Lyap = new LyapunovGenerator(socksender);
            Lyap.SetRemote(LyapunovGenerator.TypeofRemote.Reciever);
        }

        private void net_radio_CheckedChanged(object sender, EventArgs e)
        {
            net_group.Enabled = net_radio.Checked;
            server_group.Enabled = server_radio.Checked;
        }
        //public void OnClientConnect(IAsyncResult asyn)
        //{
        //    try
        //    {
        //        Socket workerSocket = m_mainSocket.EndAccept(asyn);
        //        Interlocked.Increment(ref m_clientCount);
        //        m_workerSocketList.Add(workerSocket);
        //        string msg = "Welcome client " + m_clientCount + "\n";
        //        SendMsgToClient(msg, m_clientCount);
        //        UpdateClientListControl();
        //        WaitForData(workerSocket, m_clientCount);
        //        m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        //    }
        //    catch (ObjectDisposedException)
        //    {
        //        System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
        //    }
        //    catch (SocketException se)
        //    {
        //        MessageBox.Show(se.Message);
        //    }
        //}

        //public class SocketPacket
        //{
        //    public SocketPacket(System.Net.Sockets.Socket socket, int clientNumber)
        //    {
        //        m_currentSocket = socket;
        //        m_clientNumber = clientNumber;
        //    }
        //    public System.Net.Sockets.Socket m_currentSocket;
        //    public int m_clientNumber;
        //    public byte[] dataBuffer = new byte[1024];
        //}
        //public void WaitForData(System.Net.Sockets.Socket soc, int clientNumber)
        //{
        //    try
        //    {
        //        if (pfnWorkerCallBack == null)
        //        {
        //            pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
        //        }
        //        SocketPacket theSocPkt = new SocketPacket(soc, clientNumber);

        //        soc.BeginReceive(theSocPkt.dataBuffer, 0,
        //            theSocPkt.dataBuffer.Length,
        //            SocketFlags.None,
        //            pfnWorkerCallBack,
        //            theSocPkt);
        //    }
        //    catch (SocketException se)
        //    {
        //        MessageBox.Show(se.Message);
        //    }
        //}
        //public void OnDataReceived(IAsyncResult asyn)
        //{
        //    SocketPacket socketData = (SocketPacket)asyn.AsyncState;
        //    try
        //    {
        //        int iRx = socketData.m_currentSocket.EndReceive(asyn);
        //        char[] chars = new char[iRx + 1];
        //        System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
        //        int charLen = d.GetChars(socketData.dataBuffer, 0, iRx, chars, 0);

        //        System.String szData = new System.String(chars);
        //        string msg = "" + socketData.m_clientNumber + ":";
        //        AppendToRichEditControl(msg + szData);

        //        string replyMsg = "Server Reply:" + szData.ToUpper();
        //        byte[] byData = System.Text.Encoding.ASCII.GetBytes(replyMsg);

        //        Socket workerSocket = (Socket)socketData.m_currentSocket;
        //        workerSocket.Send(byData);

        //        WaitForData(socketData.m_currentSocket, socketData.m_clientNumber);

        //    }
        //    catch (ObjectDisposedException)
        //    {
        //        System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
        //    }
        //    catch (SocketException se)
        //    {
        //        if (se.ErrorCode == 10054)
        //        {
        //            string msg = "Client " + socketData.m_clientNumber + " Disconnected" + "\n";
        //            AppendToRichEditControl(msg);

        //            m_workerSocketList[socketData.m_clientNumber - 1] = null;
        //            UpdateClientListControl();
        //        }
        //        else
        //        {
        //            MessageBox.Show(se.Message);
        //        }
        //    }
        //}

        //private void send_message(string message)
        //{
        //    byte[] msg = System.Text.Encoding.ASCII.GetChars(message);
        //    Socket socksender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
        //    //IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 4000);
        //    socksender.Send(msg, SocketFlags.Broadcast);
        //}

        //private void send_message(string message, int client)
        //{

        //}
        //private void send_message_toall(string message)
        //{
        //    try
        //    {
        //        byte[] byData = System.Text.Encoding.ASCII.GetBytes(message);
        //        Socket workerSocket = null;
        //        for (int i = 0; i < m_workerSocketList.Count; i++)
        //        {
        //            workerSocket = (Socket)m_workerSocketList[i];
        //            if (workerSocket != null)
        //            {
        //                if (workerSocket.Connected)
        //                {
        //                    workerSocket.Send(byData);
        //                }
        //            }
        //        }
        //    }
        //    catch (SocketException se)
        //    {
        //        MessageBox.Show(se.Message);
        //    }

        //}
    }
}
