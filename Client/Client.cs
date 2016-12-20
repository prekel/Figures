using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var ID = Console.ReadLine();
			Console.WriteLine(String.Format("My ID-{0} \n----------------------------------------------", ID));
			var ip = IPAddress.Parse("127.0.0.1");
			var sock1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sock1.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, 1);
			var iep1 = new IPEndPoint(ip, 9060);
			var flag = true;
			//sock1.Connect(iep1);
			do
			{
				var str = Console.ReadLine();
				if (String.IsNullOrEmpty(str))
				{
					continue;
				}
				if (str == "exit")
				{
					flag = false;
				}
				byte[] data1 = Encoding.Default.GetBytes(ID + ": " + str);
				sock1.SendTo(data1, iep1);
				//sock1.Send(data1);
			} while (flag);
			sock1.Close();
			Console.ReadKey();
		}
	}
}