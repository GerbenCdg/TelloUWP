using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TelloLib.CustomEventArgs;

namespace TelloLib
{
    public class CommunicationManager
    {
        private bool _isConnected = false;

        private const string _ipString = "192.168.10.1";
        private const int _port = 8889;

        private const string _videoStreamIp = "127.0.0.1";
        private const int _videoStreamPort = 7038;

        public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusHandler;
        public event EventHandler<VideoStreamEventArgs> VideoStreamHandler;
        // TODO invocate streamhandler when a frame arrives
        public event EventHandler<DroneInfoEventArgs> DroneInfoHandler;

        private UdpClient TelloClient { get; set; }
        private UdpClient StreamClient { get; set; }

        internal CommunicationManager()
        {
            TelloClient = new UdpClient();
            TelloClient.Client.Connect(IPAddress.Parse(_ipString), _port);

            StreamClient = new UdpClient(new IPEndPoint(IPAddress.Parse(_videoStreamIp), _videoStreamPort));
        }

        internal bool IsConnected()
        {
            return _isConnected;
        }

        internal void Disconnect()
        {
            try
            {
                TelloClient.Client.Dispose();
                StreamClient.Client.Dispose();
            }
            catch (Exception e1)
            {

            }
        }

        internal void Send(byte[] packet)
        {
            TelloClient.Client.Send(packet);
        }

        internal void SendCommand(string command)
        {
            // Enable command mode
            Send(Encoding.ASCII.GetBytes("command"));
            // Send the actual command
            Send(Encoding.ASCII.GetBytes(command.ToLower()));
        }

        internal void ConnectAndStartListening()
        {
            StartListening();

            byte[] connectionPacket = Encoding.UTF8.GetBytes("conn_req:\x00\x00");
            connectionPacket[connectionPacket.Length - 2] = 0x96;
            connectionPacket[connectionPacket.Length - 1] = 0x17;

            ConnectionStatusHandler.Invoke(this, new ConnectionStatusEventArgs(ConnectionStatus.Connecting));
            TelloClient.Client.Send(connectionPacket);
        }

        private void StartListening()
        {
            ListenTello();
            // ListenVideoStream();
        }

        private void ListenTello()
        {
            // TODO handle timeouts
            Task.Run(async () =>
            {
                while (true)
                {
                    var received = await TelloClient.ReceiveAsync();
                    var receivedStr = Encoding.ASCII.GetString(received.Buffer);

                    if (receivedStr.StartsWith("conn_ack"))
                    {
                        _isConnected = true;
                        ConnectionStatusHandler(this, new ConnectionStatusEventArgs(ConnectionStatus.Connected));
                    }

                }
            });
        }

        private void ListenVideoStream()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var received = await StreamClient.ReceiveAsync();
                }
            });
        }

    }
}
