using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FiguresServer;

namespace Client
{
	public class Client
	{
		public Connection Connect, rConnect;
		
		public static void Main(string[] args)
		{
			var ip = IPAddress.Parse(args[0]);
			var port = int.Parse(args[1]);
			var rport = int.Parse(args[2]);
			var client = new Client(ip, port, rport);
		}
		
		public Client(IPAddress ip, int port, int rport)
		{
			Connect = new Connection(port);
			try
			{
				Connect.Receive();
				Connect.NewMessage += NewMsg;
			}
			catch (SocketException)
			{
				rConnect = new Connection(rport);
				rConnect.Receive();
				rConnect.NewMessage += NewMsg;
			}
			
			while (true)
			{
				var str = Console.ReadLine();
				if (string.IsNullOrEmpty(str))
					continue;
				if (str == "exit")
					break;
				Connect.Send(str, ip);
			}
		}

		//public void NewMsg(EndPoint ep, string message)
		public void NewMsg(IPAddress ip, string message)
		{
			//var ip = ((IPEndPoint) ep).Address;
			Console.WriteLine(ip + " " + message);
		}
	}
}