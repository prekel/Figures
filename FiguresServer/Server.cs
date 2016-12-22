using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace FiguresServer
{
	public class Server
	{
		private Dictionary<IPAddress, int> PlayersSet = new Dictionary<IPAddress, int>();
		private int n = 0;
		Connection Connect, rConnect;

		public Player[] players = new Player[10];

		public static void Main(string[] args)
		{
			var port = int.Parse(args[0]);
			var rport = int.Parse(args[1]);

			var serv = new Server(port, rport);
			while (true)
			{

			}
		}

		public Server(int port, int rport)
		{
            Connect = new Connection(port, rport);
            Connect.NewMessage += ServerRequestHandler;
            Connect.Receive();
		}
		
		public void ServerRequestHandler(IPAddress ip, string message)
		{
			if (!PlayersSet.ContainsKey(ip))
			{
				players[n] = new Player()
				{
					Ip = ip,
					Number = n
				};
				PlayersSet[ip] = n;
				n++;
			}
			Console.WriteLine(players[PlayersSet[ip]] + " " + message);

			var player = players[PlayersSet[ip]];

			try
			{
				var request = message.Split();

				var clients = (from p in players where !Equals(player.Ip, ip) select p.Ip).ToArray();

				switch (request[0])
				{
					case "$connect":
						//players[ip] = new Player(ip);
						//Connect.Send("$newplayer" + " " + ip.ToString(), clients);
						Connect.Send("$acceptconnect", ip);
						break;
					case "$disconnect":
						players[PlayersSet[ip]] = null;
						PlayersSet[ip] = -1;
						//Connect.Send("$deleteplayer" + " " + ip.ToString(), clients);
						break;
					case "$momentumchange":
						Connect.Send("$momentumchange" + " " + request[1] + " " + request[2] + " " + request[3], clients);
						break;
					case "$add":
						Connect.Send("$add" + " " + request[1] + " " + request[2], clients);
						break;
					case "$send":
						Connect.Send(ip.ToString() + " " + message, clients);
						break;
					default:
						Connect.Send(ip.ToString() + " " + message, clients);
						break;
					//throw new Exception("Неверный запрос");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
	}
}
