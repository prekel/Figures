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

		public MainWindow()
		{
			InitializeComponent();

			MouseUp += MainWindow_MouseUp;
			AddButton.Click += AddButton_Click;
			ConnectionButton.Click += ConnectionButton_Click;
			//Body.NewLog += NewLog;

			//var r = (1280 / 325.0) * 25 / 2;
			var r = 50;
			var m = 10 / 1000.0;
			var mu = 0.1;
			var ck = 40;
            var M = 16;

			for (var i = 0; i < M / 2; i++)
			{
				bodies[i] = new Body(150 + i * 105, 150, r, m, mu, ck, Brushes.AliceBlue, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[i].Figure);
                n++;
			}

			for (var i = M / 2; i < M; i++)
			{
				bodies[i] = new Body(150 + (i - M / 2) * 105, 600, r, m, mu, ck, Brushes.Tomato, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[i].Figure);
                n++;
			}

			var fps = 100;
			var cap = 20;

			var loop = new Thread(Body.Move);
			loop.Start(new object[] { bodies, Dispatcher, fpsCount, fps, cap } );
			
		}

		public void ConnectionButton_Click(object sender, RoutedEventArgs e)
		{
			var condial = new ConnectionDialog(log);
			condial.Show();
			condial.NewLog += NewLog;
		}

		private void NewLog(string s)
		{
			log += s;
		}

		void MainWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (F)
            {
                bodies[n] = new Body(e.GetPosition(Grid).X, Grid.ActualHeight - e.GetPosition(Grid).Y, 50, 10 / 1000.0, 0.1, 40, Brushes.Cyan, Brushes.DarkOliveGreen);
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
