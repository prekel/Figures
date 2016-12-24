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
		private string _name = "", _comp;
		public IPAddress Ip;

        public int Number { get; set; }

		public string Name { get { return _name; } set { _name = value; } }
        public string CompName { get { return _comp; } set { _comp = value; } }

		public Player(string ip)
		{
			Ip = IPAddress.Parse(ip);
		}

		public Player(IPAddress ip, int number)
        {
         	Ip = ip;
	        Number = number;
        }

		public Player()
		{

		}

		public override string ToString()
		{
			return Ip.ToString() + Name;
		}
	}
}
