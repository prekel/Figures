using System;
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

namespace Figures
{
	/// <summary>
	/// Логика взаимодействия для ConnectionDialog.xaml
	/// </summary>
	public partial class ConnectionDialog : Window
	{
		public ConnectionDialog()
		{
			InitializeComponent();
			ConnectButton.Click += ConnectButton_Click;
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			Connection.Receiver(PortBox.Text);
			System.Net.Sockets.
		}
	}
}
