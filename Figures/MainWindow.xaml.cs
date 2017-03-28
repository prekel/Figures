using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net;
using FiguresServer;

namespace Figures
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		int n = 0;
		Body[] bodies = new Body[500];
		bool F;
		string log = "";
		private Connection serv, rserv;
		private IPAddress servIp;
		private ConnectionDialog condial;

		public MainWindow()
		{
			InitializeComponent();

			MouseUp += MainWindow_MouseUp;
			AddButton.Click += AddButton_Click;
			ConnectionButton.Click += ConnectionButton_Click;

			////var r = (1280 / 325.0) * 25 / 2;
			//var r = 50;
			//var m = 10 / 1000.0;
			//var mu = 0.3;
			//var ck = 45;
			//var M = 16;
			var r = 50;
			var m = 1;
			var mu = 0.3;
			var ck = 55;
			//var mu = 0.001;
			//var ck = 10;
			var M = 16;

			for (var i = 0; i < M / 2; i++)
			{
				bodies[i] = new Body(n, 150 + i * 105, 150, r, m, mu, ck, Brushes.AliceBlue, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[i].Figure);
				n++;
			}

			for (var i = M / 2; i < M; i++)
			{
				bodies[i] = new Body(n, 150 + (i - M / 2) * 105, 600, r, m, mu, ck, Brushes.Tomato, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[i].Figure);
				n++;
			}

			var fps = 60;
			var cap = 1;

			var loop = new Thread(Body.Move);
			loop.Start(new object[] { bodies, Dispatcher, fpsCount, fps, cap });
		}

		public void ConnectionButton_Click(object sender, RoutedEventArgs e)
		{
			condial = new ConnectionDialog(log);
			condial.Show();
			condial.NewLog += NewLog;
			condial.AcceptConnect += AcceptConnect;
			servIp = IPAddress.Parse(condial.IPBox.Text);
		}

		public void AcceptConnect(Connection connect)
		{
			serv = connect;
			Body.MomentumChange += MomentumChange;
			serv.NewMessage += Serv_NewMessage;
		}

		public void AcceptConnect(Connection connect, Connection rconnect)
		{
			serv = connect;
			rserv = rconnect;
			Body.MomentumChange += MomentumChange;
			serv.NewMessage += Serv_NewMessage;
		}

		private void Serv_NewMessage(IPAddress ip, string message)
		{
			NewLog(ip + " " + message + '\n');
			var request = message.Split();
			if (request[0] == "$momentumchange")
			{
				var num = int.Parse(request[1]);
				var x = double.Parse(request[2]);
				var y = double.Parse(request[3]);
				var mx = double.Parse(request[4]);
				var my = double.Parse(request[5]);
				bodies[num].X = x;
				bodies[num].Y = y;
				bodies[num].Momentum = new Vector(mx, my);
				bodies[num].StartMoveByMomentum(bodies[num].Momentum);
			}
		}

		public void MomentumChange(string s)
		{
			serv.Send(s, servIp);
		}

		private void NewLog(string s)
		{
			log += s;
			Action action = () => { condial.logBox.Text += s; };
			Dispatcher.Invoke(action);
		}

		void MainWindow_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (n == 499)
				return;
			if (F)
			{
				bodies[n] = new Body(n, e.GetPosition(Grid).X, Grid.ActualHeight - e.GetPosition(Grid).Y, 50, 100500, 0.3, 55, Brushes.Cyan, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[n].Figure);
				n++;
			}
		}

		public static Vector Norm(Vector a)
		{
			var b = a;
			b.Normalize();
			return b;
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			F = !F;
		}
	}
}
