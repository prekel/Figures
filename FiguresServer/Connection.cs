using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace FiguresServer
{
	class Connection
	{
		private int _port;
		public int Port { get { return _port; } set { _port = value; } }

		public delegate void RequestContainer(IPAddress ip, string request);
		public event RequestContainer RequestCame;
	
		public Connection(int port)
		{
			_port = port;
			var serverthread = new Thread(Receiver);
			serverthread.Start(port);
		}

		public void SendThread(object parameters)
		{
			try
			{
				var param = (object[])parameters;
				var msg = (string)param[0];
				var ips = param[1] as IPAddress[];
				var port = (int)param[2];

				foreach (var ip in ips)
				{
					var endPoint = new IPEndPoint(ip, port);
					var connector = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					connector.Connect(endPoint);
					var sendBytes = Encoding.Default.GetBytes(msg);
					connector.Send(sendBytes);
					connector.Close();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		public void Send(string msg, IPAddress[] ips, int port)
		{
			var sendthread = new Thread(SendThread);
			sendthread.Start(new object[] { msg, ips, port });
		}

		public void Receiver(object Port)
		{
			var port = (int)Port;
			var listen = new TcpListener(IPAddress.Any, port);
			listen.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			listen.Start();
			while (true)
			{
				try
				{
					var receiveSocket = listen.AcceptSocket();
					var receive = new byte[256];

					var messageR = new MemoryStream();
					do
					{
						var receivedBytes = receiveSocket.Receive(receive, receive.Length, 0);
						messageR.Write(receive, 0, receivedBytes);
					} while (receiveSocket.Available > 0);

					var request = Encoding.Default.GetString(messageR.ToArray());
					//var ip = ((IPEndPoint) receiveSocket.RemoteEndPoint).Address.ToString();
					var ip = ((IPEndPoint)receiveSocket.RemoteEndPoint).Address;

					RequestCame(ip, request);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
			}
		}
	}
}
