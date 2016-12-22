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
	public class Connection
	{
		private int _port, _rport;
		public int Port { get { return _port; } set { _port = value; } }
		public int ReservePort { get { return _rport; } set { _rport = value; } }
		private IPAddress _lip;
		public IPAddress LocalIp { get { return _lip; } set { _lip = value; } }

		private int portReceive;
		private Socket socket;
		private byte[] buffer;
		private EndPoint endPoint;

		//public delegate void MessageContainer(EndPoint ep, string message);
		public delegate void MessageContainer(IPAddress ip, string message);
		public event MessageContainer NewMessage;

		public Connection(int port, int rport)
		{
			Port = port;
			ReservePort = rport;
			var hostips = Dns.GetHostEntry(Dns.GetHostEntry("localhost").HostName).AddressList;
			foreach (var ip in hostips)
			{
				if (ip.ToString().Substring(0, 7) == "192.168")
				{
					LocalIp = ip;
					break;
				}
			}
		}

		public Connection(int port)//, int rport)
		{
			Port = port;
			//ReservePort = rport;
			//LocalIp = Dns.GetHostEntry(Dns.GetHostEntry("localhost").HostName).AddressList[1];
		}

		public void Receive()
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			buffer = new byte[1024];
			portReceive = Port;
			endPoint = new IPEndPoint(IPAddress.Any, portReceive);

			socket.Bind(endPoint);
			socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, ReceiveCallback, socket);
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			var n = socket.EndReceiveFrom(ar, ref endPoint);
			var receivestring = Encoding.Default.GetString(buffer, 0, n);
			NewMessage?.Invoke(((IPEndPoint)endPoint).Address, receivestring);

			endPoint = new IPEndPoint(IPAddress.Any, portReceive);
			socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, ReceiveCallback, socket);
		}

		public void Send(string message, IPAddress ip)
		{
			new Thread(SendThread).Start(new object[] { message, new IPAddress[] { ip } });
		}

		public void Send(string message, IPAddress[] ips)
		{
			new Thread(SendThread).Start(new object[] { message, ips });
		}

		public void SendThread(object parameters)
		{
			var param = (object[])parameters;
			var message = (string)param[0];
			var ips = (IPAddress[])param[1];

			var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			//sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, false);
			foreach (var ip in ips)
			{
				var data = Encoding.Default.GetBytes(message);
				var iep = Equals(ip, LocalIp) ? new IPEndPoint(ip, ReservePort) : new IPEndPoint(ip, Port);
				//var iep = new IPEndPoint(ip, Port);
				sock.SendTo(data, iep);
			}
			sock.Close();
		}

		public override string ToString()
		{
			return Port.ToString() + ((ReservePort != 0) ? " " + ReservePort.ToString() : "");
		}
	}
}
