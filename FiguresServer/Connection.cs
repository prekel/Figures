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

		public delegate void MessageContainer(Player player, string message);
		public event MessageContainer NewMessage;

		public delegate void StreamContainer(IPAddress ip, StreamReader sr, StreamWriter sw);
		public event StreamContainer NewClient;

		//public Dictionary<IPAddress, StreamReader> SrDict = new Dictionary<IPAddress, StreamReader>();
		//public Dictionary<IPAddress, StreamWriter> SwDict = new Dictionary<IPAddress, StreamWriter>();

		public Connection(int port)
		{
			_port = port;
			//var serverthread = new Thread(Receiver);
			var serverthread = new Thread(GetStreams);
			serverthread.Start(port);
		}

		//private void SendThread(object parameters)
		//{
		//	TcpClient client = new TcpClient();
		//	try
		//	{
		//		var param = (object[])parameters;
		//		var msg = (string)param[0];
		//		var ips = param[1] as IPAddress[];
		//		var port = (int)param[2];

		//		client.Connect(ips[0], port);
		//		StreamWriter sw = new StreamWriter(client.GetStream());
		//		sw.AutoFlush = true;
		//		sw.WriteLine(msg);
		//	}
		//	catch (Exception ex)
		//	{
		//		System.Diagnostics.Debug.WriteLine(ex.Message);
		//	}
		//	client.Close();
		//}

		public void GetStreams(object Port)
		{
			var port = (int)Port;
			TcpListener listener = new TcpListener(IPAddress.Any, port);
			listener.Start();
			while (true)
			{
				if (listener.Pending())
				{
					var client = listener.AcceptTcpClient();
					var sr = new StreamReader(client.GetStream());
					var sw = new StreamWriter(client.GetStream());
					var ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
					NewClient(ip, sr, sw);
					//SrDict[ip] = sr;
					//SwDict[ip] = sw;
				}
			}
		}

		public void Send(string msg, Player[] players)
		{
			var sendthread = new Thread(SendThread);
			sendthread.Start(new object[] { msg, players } );
		}

		public void SendThread(object parameters)
		{
			try
			{
				var param = (object[])parameters;
				var msg = (string)param[0];
				var players = (Player[])param[1];
				foreach (var player in players)
				{
					player.Writer.WriteLine(msg);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		public void Receive(Player[] players)
		{
			var receivethread = new Thread(ReceiveThread);
			receivethread.Start(new object[] { players });
		}

		public void ReceiveThread(object parameters)
		{
			try
			{
				while (true)
				{
					var param = (object[])parameters;
					var players = (Player[])param[0];
					foreach (var player in players)
					{
						if (player == null)
							continue;
						if (player.Reader.EndOfStream)
							continue;
						var s = player.Reader.ReadLine();
						Console.WriteLine(s);
						Debug.WriteLine(s);
						NewMessage(player, s);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		//public void Send(string msg, IPAddress[] ips, int port)
		//{
		//	var sendthread = new Thread(SendThread);
		//	sendthread.Start(new object[] { msg, ips, port });
		//}

		//public void SendThread(object parameters)
		//{
		//	try
		//	{
		//		var param = (object[])parameters;
		//		var msg = (string)param[0];
		//		var ips = param[1] as IPAddress[];
		//		var port = (int)param[2];

		//		foreach (var ip in ips)
		//		{
		//			var endPoint = new IPEndPoint(ip, port);
		//			var connector = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		//			connector.Connect(endPoint);
		//			var sendBytes = Encoding.Default.GetBytes(msg);
		//			connector.Send(sendBytes);
		//			connector.Close();
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		System.Diagnostics.Debug.WriteLine(ex.Message);
		//	}
		//}

		//public void Receiver(object Port)
		//{
		//	var port = (int)Port;
		//	var listen = new TcpListener(IPAddress.Any, port);
		//	listen.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		//	listen.Start();
		//	while (true)
		//	{
		//		try
		//		{
		//			var receiveSocket = listen.AcceptSocket();
		//			var receive = new byte[256];

		//			var messageR = new MemoryStream();
		//			do
		//			{
		//				var receivedBytes = receiveSocket.Receive(receive, receive.Length, 0);
		//				messageR.Write(receive, 0, receivedBytes);
		//			} while (receiveSocket.Available > 0);

		//			var message = Encoding.Default.GetString(messageR.ToArray());
		//			//var ip = ((IPEndPoint) receiveSocket.RemoteEndPoint).Address.ToString();
		//			var ip = ((IPEndPoint)receiveSocket.RemoteEndPoint).Address;

		//			RequestCame(ip, message);
		//		}
		//		catch (Exception ex)
		//		{
		//			System.Diagnostics.Debug.WriteLine(ex.Message);
		//		}
		//	}
		//}
	}
}
