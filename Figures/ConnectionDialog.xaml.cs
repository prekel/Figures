﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using FiguresServer;

namespace Figures
{
	/// <summary>
	/// Логика взаимодействия для ConnectionDialog.xaml
	/// </summary>
	public partial class ConnectionDialog : Window
	{
		public delegate void ConnectionContainer(Connection connect);
		public event ConnectionContainer AcceptConnect;

		public delegate void StringContainer(string s);
		public event StringContainer NewLog;

		Connection serv, rserv;
		string log;

		public ConnectionDialog()
		{
			InitializeComponent();
			ConnectButton.Click += ConnectButton_Click;
			CloseButton.Click += CloseButton_Click;
			logBox.TextChanged += LogBox_TextChanged;
		}

		public ConnectionDialog(string log)
		{
			InitializeComponent();
			ConnectButton.Click += ConnectButton_Click;
			CloseButton.Click += CloseButton_Click;

			logBox.TextChanged += LogBox_TextChanged;
			logBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			logBox.Text = log;
		}


		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			if (PortBox.Text.ToString().Split().Length == 2)
			{
				var port = int.Parse(PortBox.Text.ToString().Split()[0]);
				var rport = int.Parse(PortBox.Text.ToString().Split()[1]);
				serv = new Connection(port);
				try
				{
					serv.Receive();
					serv.NewMessage += Serv_NewMessage;
				}
				catch (SocketException)
				{
					rserv = new Connection(rport);
					rserv.Receive();
					rserv.NewMessage += Serv_NewMessage;
				}
			}
			else
			{
				serv = new Connection(int.Parse(PortBox.Text.ToString()));
				serv.Receive();
				serv.NewMessage += Serv_NewMessage;
			}
			var s = "$connect\n";
			serv.Send(s, IPAddress.Parse(IPBox.Text));
			logBox.Text += s;
			NewLog?.Invoke(s);
		}

		private void Serv_NewMessage(IPAddress ip, string message)
		{
			NewLog?.Invoke(ip.ToString() + " " + message + "\n");
			Action action = () => { logBox.Text += ip.ToString() + " " + message + "\n"; };
			Dispatcher.Invoke(action);
			if (message == "$acceptconnect")
			{
				serv.NewMessage -= Serv_NewMessage;
				AcceptConnect?.Invoke(serv);
			}
		}

		private void LogBox_TextChanged(object sender, EventArgs e)
		{
			logBox.ScrollToEnd();
		}
	}
}
