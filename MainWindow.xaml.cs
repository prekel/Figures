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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Figures
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var bodies = new Body[10];
			var disp = Dispatcher;

			//			bodies[0] = new Body(330, 170, 70, 100, 0.1, 0.001, 20, ref bodies, ref disp);
			//			Grid.Children.Add(bodies[0].Figure);
			//
			//			bodies[1] = new Body(330, 370, 70, 100, 0.1, 0.001, 50, ref bodies, ref disp);
			//			Grid.Children.Add(bodies[1].Figure);
			//
			//			bodies[2] = new Body(600, 500, 100, 100, 0.5, 0.002, 10, ref bodies, ref disp);
			//			Grid.Children.Add(bodies[2].Figure);
			//
			//			bodies[3] = new Body(900, 500, (25.0 / 2) * (1280 / 325.0), 100, 6.45 / 1000, 0.005, 30, ref bodies, ref disp);
			//			Grid.Children.Add(bodies[3].Figure);
			//
			//			bodies[4] = new Body(1000, 400, 50, 100, 6.45 / 1000, 0.01, 30, ref bodies, ref disp);
			//			Grid.Children.Add(bodies[4].Figure);

			//			bodies[0] = new Body(200, 300, 100, 100, 1, 0.01, 30, ref bodies, Brushes.Blue, Brushes.DarkOliveGreen, ref disp);
			//			Grid.Children.Add(bodies[0].Figure);
			//
			//			bodies[1] = new Body(500, 300, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			//			Grid.Children.Add(bodies[1].Figure);
			//
			//			bodies[2] = new Body(800, 300, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			//			Grid.Children.Add(bodies[2].Figure);
			//
			//			bodies[3] = new Body(800, 600, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			//			Grid.Children.Add(bodies[3].Figure);
			//
			//			bodies[4] = new Body(500, 600, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			//			Grid.Children.Add(bodies[4].Figure);

			bodies[0] = new Body(200, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[0].Figure);

			bodies[1] = new Body(300, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[1].Figure);

			bodies[2] = new Body(400, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[2].Figure);

			bodies[3] = new Body(500, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[3].Figure);

			bodies[4] = new Body(600, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[4].Figure);


			bodies[5] = new Body(200, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[5].Figure);

			bodies[6] = new Body(300, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[6].Figure);

			bodies[7] = new Body(400, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[7].Figure);

			bodies[8] = new Body(500, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[8].Figure);

			bodies[9] = new Body(700, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[9].Figure);

		}

		//public void MoveEllipse(Ellipse el, Thickness margin)
		//{
		//	Action action = () =>
		//	{
		//		el.Margin = margin;
		//	};
		//	Invoke(action);
		//}

		//public int Ret1()
		//{
		//	return 1;
		//}

		//private void Mouse_Move(object sender, MouseEventArgs e)
		//{
		//	var xy = Mouse.GetPosition(grid);
		//	var b = Ellipse1.Margin;
		//	b.Top = xy.Y - 50;
		//	b.Left = xy.X - 50;
		//	Ellipse1.Margin = b;
		//}
	}
}
