using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace FiguresServer
{
	public class Server
	{
		private Dictionary<IPAddress, Player> players = new Dictionary<IPAddress, Player>();
		private int _port;
		public int Port { get { return _port; } set { _port = value; } }
		Connection Connect;

		public static void Main(string[] args)
		{
			var file = args[0];
			var port = int.Parse(args[1]);
			var serv = new Server(port);
		}

		public Server(int port)
		{
			Port = port;
			Connect = new Connection(Port);
			Connect.RequestCame += ServerRequestHandler;
		}

		public void ServerRequestHandler(IPAddress ip, string req)
		{
			var request = req.Split();
			try
			{
				var ipstosend = new List<IPAddress>();
				foreach (var player in players)
				{
					if (Equals(player.Value.Ip, ip))
						continue;
					ipstosend.Add(player.Value.Ip);
				}
				var ips = ipstosend.ToArray();
				switch (request[0])
				{
					case "$connect":
						players[ip] = new Player(ip);
						Connect.Send("$newplayer" + " " + ip.ToString(), ips, Port);
						break;
					case "$disconnect":
						players[ip] = null;
						Connect.Send("$deleteplayer" + " " + ip.ToString(), ips, Port);
						break;
					case "$momentumchange":
						Connect.Send("$momentumchange" + " " + request[1] + " " + request[2] + " " + request[3], ips, Port);
						break;
					case "$add":
						Connect.Send("$add" + " " + request[1] + " " + request[2], ips, Port);
						break;
					default:
						throw new Exception("Неверный запрос");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
	}
}
