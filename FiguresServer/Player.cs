using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace FiguresServer
{
	/// <summary> Description of Player. </summary>
	public class Player
	{
		string _name, _comp;
		public IPAddress Ip;

		public StreamReader Reader;
		public StreamWriter Writer;

		public Player(string ip)
		{
			Ip = IPAddress.Parse(ip);
		}

		public Player(IPAddress ip)
		{
			Ip = ip;
		}

		public Player()
		{

		}

		public Player(int port)
		{
			var Port = port;
			var Connect = new Connection(Port);
			Connect.Send("12312311111", new Player[] { new Player() { Writer = new StreamWriter(new NetworkStream(new Socket((new IPEndPoint(IPAddress.Parse("192.168.1.4"), port)).AddressFamily, SocketType.Stream, ProtocolType.Tcp))) } });
		}

		public override string ToString()
		{
			return Ip.ToString();
		}
	}
}
