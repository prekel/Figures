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
		private Dictionary<IPAddress, int> PlayersSet = new Dictionary<IPAddress, int>();
		private int _port, n = 0;
		public int Port { get { return _port; } set { _port = value; } }
		Connection Connect;

		public Player[] players = new Player[10];

		//public Dictionary<IPAddress, StreamReader> SrDict = new Dictionary<IPAddress, StreamReader>();
		//public Dictionary<IPAddress, StreamWriter> SwDict = new Dictionary<IPAddress, StreamWriter>();

		public static void Main(string[] args)
		{
			var file = args[0];
			var port = int.Parse(args[1]);
			var s = Console.ReadLine();
			if (s[0] == 'c')
			{
				var client = new Player(port);
			}
			else
			{
				var serv = new Server(port);
			}
		}

		public Server(int port)
		{
			Port = port;
			Connect = new Connection(Port);
			Connect.GetStreams(port);
			Connect.NewClient += Connect_NewClient;
			Connect.NewMessage += ServerRequestHandler;
			Connect.Receive(players);
		}

		private void Connect_NewClient(IPAddress ip, StreamReader sr, StreamWriter sw)
		{
			//SrDict[ip] = sr;
			//SwDict[ip] = sw;
			//players[ip].Reader = sr;
			//players[ip].Writer = sw;
			if (PlayersSet.ContainsKey(ip))
			{
				players[PlayersSet[ip]].Reader = sr;//, Writer = sw };
				return;
			}
			PlayersSet[ip] = n;
			players[n] = new Player() { Ip = ip, Reader = sr, Writer = sw };
			n++;
		}

		public void ServerRequestHandler(Player player, string message)
		{
			var request = message.Split();
			try
			{
				Console.WriteLine(player + " " + message);
				//var playerstosend = new List<StreamWriter>();
				//foreach (var player1 in players)
				//{
				//	//if (Equals(player.Value.Ip, ip))
				//	//	continue;
				//	//if (Equals(player1.Value.Reader, player.Reader))
				//	//	continue;
				//	playerstosend.Add(player1.Value.Writer);
				//}
				//var clients = playerstosend.ToArray();
				//switch (request[0])
				//{
				//	case "$connect":
				//		players[ip] = new Player(ip);
				//		Connect.Send("$newplayer" + " " + ip.ToString(), clients, Port);
				//		break;
				//	case "$disconnect":
				//		players[ip] = null;
				//		Connect.Send("$deleteplayer" + " " + ip.ToString(), clients, Port);
				//		break;
				//	case "$momentumchange":
				//		Connect.Send("$momentumchange" + " " + request[1] + " " + request[2] + " " + request[3], clients, Port);
				//		break;
				//	case "$add":
				//		Connect.Send("$add" + " " + request[1] + " " + request[2], clients, Port);
				//		break;
				//	default:
				//		throw new Exception("Неверный запрос");
				//}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
	}
}
