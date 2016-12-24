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
		private static int portReceive;
		private static Socket socket;
		private static byte[] buffer;
		
		public delegate void MessageContainer(EndPoint ep, string message);
		public static event MessageContainer NewMessage;

		private static EndPoint endPoint;

		public static void Receive(int port)
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			portReceive = 7777;
			buffer = new byte[1024];
			endPoint = new IPEndPoint(IPAddress.Any, portReceive);

			socket.Bind(endPoint);
			socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(ReceiveCallback), socket);
		}

		private static void ReceiveCallback(IAsyncResult ar)
		{
			var n = socket.EndReceiveFrom(ar, ref endPoint);
			var receivestring = Encoding.Default.GetString(buffer, 0, n);
			if (NewMessage != null) NewMessage(endPoint, receivestring);
			endPoint = new IPEndPoint(IPAddress.Any, portReceive);
			socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(ReceiveCallback), socket);
		}
		
		public static void Send(string message, IPAddress ip, int port, string ID)
		{
			var sock1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sock1.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, false);
			var iep1 = new IPEndPoint(ip, port);
			var data1 = Encoding.Default.GetBytes(ID + ": " + message);
			sock1.SendTo(data1, iep1);
			sock1.Close();
		}
	}



	class Program
	{
		private static Dictionary<IPAddress, int> PlayersSet = new Dictionary<IPAddress, int>();
		public static Player[] players = new Player[100];
		public static int n = 0;
		static void Main(string[] args)
		{
			var port = int.Parse(args[0]);

			//var s = new Connection();
			Connection.Receive(port);
			Connection.NewMessage += S_NewMessage;

			var ID = Console.ReadLine();
			Console.WriteLine(string.Format("My ID-{0} \n----------------------------------------------", ID));

			var flag = true;
			do
			{
				var str = Console.ReadLine();
				if (string.IsNullOrEmpty(str))
				{
					continue;
				}
				if (str == "exit")
				{
					flag = false;
				}
				foreach (var player in players)
				{
					if (player == null)
						continue;
					Connection.Send(str, player.Ip, port, ID);
				}
			} while (flag);

			Console.ReadKey();
		}

		private static void S_NewMessage(EndPoint ep, string message)
		{
			var ip = ((IPEndPoint) ep).Address;
			if (!PlayersSet.ContainsKey(ip))
			{
				players[n] = new Player(ip);
				players[n].number = n;
				PlayersSet[ip] = n;
				n++;
			}
			Console.WriteLine(ep + " " + message);
		}
	}
}