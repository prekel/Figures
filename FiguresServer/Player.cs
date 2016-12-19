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

		public override string ToString()
		{
			return Ip.ToString();
		}
	}
}
