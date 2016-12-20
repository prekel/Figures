using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
	public class Connection
	{
		private string host = "127.0.0.1";
		private static int portReceive = 9060;
		private int portSend = 9061;
		private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		private byte[] buffer = new byte[1024];
		
		public delegate void MessageContainer(EndPoint ep, string message);
		public event MessageContainer NewMessage;

		private static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, portReceive);
		private EndPoint endPoint = (EndPoint)ipEndPoint;

		public void Start()
		{
			socket.Bind(endPoint);
			socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(ReceiveCallback), socket);
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			var n = socket.EndReceiveFrom(ar, ref endPoint);
			var receivestring = Encoding.Default.GetString(buffer, 0, n);
			NewMessage(endPoint, receivestring);
			endPoint = new IPEndPoint(IPAddress.Any, portReceive);
			socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(ReceiveCallback), socket);
		}
		
		public void Send(string message, IPAddress ip, int port)
		{
			//var ip = IPAddress.Parse("127.0.0.1");

			var sock1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sock1.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, false);
			var iep1 = new IPEndPoint(ip, 9060);
			var flag = true;
			do
			{
				var str = Console.ReadLine();
				if (String.IsNullOrEmpty(str))
				{
					continue;
				}
				if (str == "exit")
				{
					flag = false;
				}
				byte[] data1 = Encoding.Default.GetBytes(ID + ": " + str);
				sock1.SendTo(data1, iep1);
			} while (flag);
			sock1.Close();
		}
	}



	class Program
	{
		static void Main(string[] args)
		{
			var s = new Connection();
			s.Start();
			s.NewMessage += S_NewMessage;

			var ID = Console.ReadLine();
			Console.WriteLine(String.Format("My ID-{0} \n----------------------------------------------", ID));
			
			Console.ReadKey();
		}

		private static void S_NewMessage(EndPoint ep, string message)
		{
			Console.WriteLine(ep + " " + message);
		}
	}
}