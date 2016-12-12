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

			var bodies = new Body[4];
			var disp = Dispatcher;

			bodies[0] = new Body(330, 170, 70, 100, 0.1, 0.001, 20, ref bodies, ref disp);
			Grid.Children.Add(bodies[0].Figure);

			bodies[1] = new Body(330, 370, 70, 100, 0.1, 0.001, 50, ref bodies, ref disp);
			Grid.Children.Add(bodies[1].Figure);

			bodies[2] = new Body(600, 500, 100, 100, 0.5, 0.002, 10, ref bodies, ref disp);
			Grid.Children.Add(bodies[2].Figure);

			bodies[3] = new Body(900, 500, (25.0 / 2) * (1280 / 325.0), 100, 6.45 / 1000, 0.005, 30, ref bodies, ref disp);
			Grid.Children.Add(bodies[3].Figure);

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
