using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
	public class Player
	{
		public string name;
		public IPAddress Ip;
		public int number;

		public Player(IPAddress ip)
        {
        	Ip = ip;
        }
	}
}