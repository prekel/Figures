using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows;

namespace Figures
{
	public class Connection
	{
		public void SendVector(Vector a)
		{
			var s = a.X.ToString() + " " + a.Y.ToString();
			Send(new string[] { s });
		}
		
		public void Send(object Message)
		{
			try
			{
				var Msg = (string[])Message;
				var MessageText = Msg[0];
				var ips = new string[Msg.Length - 2];
				for (var i = 1; i < Msg.Length - 1; i++)
				{
					ips[i - 1] = Msg[i];
				}
				var port = int.Parse(Msg[Msg.Length - 1]);

				for (var i = 0; i < ips.Length; i++)
				{
					var EndPoint = new IPEndPoint(IPAddress.Parse(ips[i]), port);
					var Connector = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					Connector.Connect(EndPoint);
					var SendBytes = Encoding.Default.GetBytes(MessageText);
					Connector.Send(SendBytes);
					Connector.Close();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		protected void Receiver(object Port)
		{
			var port = (int)Port;
			var Listen = new TcpListener(port);
			Listen.Start();
			Socket ReceiveSocket;
			//while (!_shouldReceiverStop[port])
			while (true)
			{
				try
				{
					ReceiveSocket = Listen.AcceptSocket();
					//if (_shouldReceiverStop[port])
					//{
					//	_shouldReceiverStop[port] = false;
					//	return;
					//}
					var Receive = new Byte[256];
					using (var MessageR = new MemoryStream())
					{
						int ReceivedBytes;
						do
						{
							ReceivedBytes = ReceiveSocket.Receive(Receive, Receive.Length, 0);
							MessageR.Write(Receive, 0, ReceivedBytes);
							//if (_shouldReceiverStop[port])
							//{
							//	_shouldReceiverStop[port] = false;
							//	return;
							//}
						} while (ReceiveSocket.Available > 0);
						//Thread.Sleep(100);
						//SendMsg(((IPEndPoint)ReceiveSocket.RemoteEndPoint).Address.ToString() + "→ " + Encoding.Default.GetString(MessageR.ToArray()), ChatBox);
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
			}
			//_shouldReceiverStop[port] = false;
		}
	}
}
